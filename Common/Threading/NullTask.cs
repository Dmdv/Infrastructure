using System;
using System.Threading.Tasks;
using Net.Common.Functionals;

namespace Net.Common.Threading
{
	public static class NullTask
	{
		private static readonly Func<Exception, Task> _createNullChildTaskWithException =
			new Func<Exception, Task>(
				exception =>
				{
					var tcs = new TaskCompletionSource<object>(TaskCreationOptions.AttachedToParent);
					tcs.SetException(exception);
					return tcs.Task;
				})
				.MemoizeConcurrent();

		private static readonly Task _instance;

		static NullTask()
		{
			var taskCompletionSource = new TaskCompletionSource<object>();
			taskCompletionSource.SetResult(null);
			_instance = taskCompletionSource.Task;
		}

		public static Task Instance
		{
			get { return _instance; }
		}

		public static Task ChildInstanceWithException<TException>(TException exception) where TException : Exception
		{
			return _createNullChildTaskWithException(exception);
		}
	}
}