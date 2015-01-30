using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Annotations;

namespace Net.Common.Threading
{
	public static class TaskExtensions
	{
		public static IAsyncResult AsAsyncResult(
			this Task task,
			AsyncCallback callback,
			[CanBeNull] object state)
		{
			return task.AsAsyncResult<object>(() => null, callback, state);
		}

		public static IAsyncResult AsAsyncResult<T>(
			this Task<T> task,
			AsyncCallback callback,
			[CanBeNull] object state)
		{
			return task.AsAsyncResult(() => task.Result, callback, state);
		}

		public static void Complete<T>(this Task<T> task, TaskCompletionSource<T> taskCompletionSource)
		{
			task.Complete(taskCompletionSource, () => task.Result);
		}

		public static Task ContinueOnSuccess(this Task parentTask, Action action)
		{
			return
				parentTask.ContinueWith(
					previousTask => action(),
					TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.AttachedToParent);
		}

		public static Task ContinueOnSuccess<TResult>(this Task<TResult> parentTask, Action<TResult> action)
		{
			return
				parentTask.ContinueWith(
					previousTask => action(previousTask.Result),
					TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.AttachedToParent);
		}

		public static Task<TContinuationResult> ContinueOnSuccess<TResult, TContinuationResult>(
			this Task<TResult> parentTask,
			Func<TResult, TContinuationResult> action)
		{
			return
				parentTask.ContinueWith(
					previousTask => action(previousTask.Result),
					TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.AttachedToParent);
		}

		public static AggregateException GetTasksExceptions<TResult>(this IEnumerable<Task<TResult>> tasks)
		{
			return tasks.ToNonGenericTaskArray().GetTasksExceptions();
		}

		public static void ThrowTaskExceptionIfAny(this Task task)
		{
			if (task.Exception != null)
			{
				throw task.Exception;
			}
		}

		public static void ThrowTasksExceptionsIfAny<TResult>(this IEnumerable<Task<TResult>> tasks)
		{
			tasks.ToNonGenericTaskArray().ThrowTasksExceptionsIfAny();
		}

		public static void ThrowTasksExceptionsIfAny(this IEnumerable<Task> tasks)
		{
			var aggregateException = GetTasksExceptions(tasks);
			if (aggregateException != null)
			{
				throw aggregateException;
			}
		}

		public static void WaitForFirstExceptionIfAny(
			this Task task,
			Action<Exception> doWithException,
			bool propagateException)
		{
			try
			{
				task.Wait();
			}
			catch (AggregateException exception)
			{
				var firstException = exception.Flatten().InnerExceptions.First();
				doWithException(firstException);
				if (propagateException)
				{
					throw firstException;
				}
			}
		}

		private static IAsyncResult AsAsyncResult<T>(
			this Task task,
			Func<T> getTaskResult,
			AsyncCallback callback,
			object state)
		{
			var taskCompletionSource = new TaskCompletionSource<T>(state);
			task.ContinueWith(
				previousTask =>
				{
					previousTask.Complete(taskCompletionSource, getTaskResult);
					callback(taskCompletionSource.Task);
				},
				TaskContinuationOptions.ExecuteSynchronously);

			return taskCompletionSource.Task;
		}

		private static void Complete<T>(this Task task, TaskCompletionSource<T> taskCompletionSource, Func<T> getTaskResult)
		{
			if (task.IsFaulted)
			{
				taskCompletionSource.TrySetException(task.Exception.InnerExceptions);
			}
			else if (task.IsCanceled)
			{
				taskCompletionSource.TrySetCanceled();
			}
			else
			{
				taskCompletionSource.TrySetResult(getTaskResult());
			}
		}

		private static AggregateException GetTasksExceptions(this IEnumerable<Task> tasks)
		{
			var exceptions = tasks
				.Where(task => task.Exception != null)
				.Select(task => task.Exception)
				.ToList();

			return exceptions.Any() ? new AggregateException(exceptions) : null;
		}

		private static IEnumerable<Task> ToNonGenericTaskArray<TResult>(this IEnumerable<Task<TResult>> tasks)
		{
			return tasks.Select(task => (Task) task).ToArray();
		}
	}
}