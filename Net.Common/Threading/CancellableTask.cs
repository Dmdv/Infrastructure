using System;
using System.Threading;
using System.Threading.Tasks;
using Net.Common.Contracts;

namespace Net.Common.Threading
{
	[Concurrent]
	internal sealed class CancellableTask : ICancellableTask
	{
		public CancellableTask(Action<CancellationToken> action)
		{
			_taskCancellation = new CancellationTokenSource();
			_task =
				Task.Factory.StartNew(
					() => action(_taskCancellation.Token),
					_taskCancellation.Token,
					TaskCreationOptions.LongRunning,
					TaskScheduler.Default);
		}

		public void CancelAndStop()
		{
			lock (_syncRoot)
			{
				Guard.CheckNotDisposed(_isDisposed, this);

				_taskCancellation.Cancel();
				try
				{
					_task.Wait();
				}
				catch (AggregateException exception)
				{
					exception.Flatten().Handle(e => e is TaskCanceledException);
				}
			}
		}

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}

			Guard.CheckTrue(!IsRunning(), () => new InvalidOperationException("Task is still running."));

			lock (_syncRoot)
			{
				_isDisposed = true;
				_taskCancellation.Dispose();
				_task.Dispose();
			}
		}

		private bool IsRunning()
		{
			return !_task.IsCanceled && !_task.IsCompleted && !_task.IsFaulted;
		}

		private bool _isDisposed;
		private readonly object _syncRoot = new object();
		private readonly Task _task;
		private readonly CancellationTokenSource _taskCancellation;
	}
}
