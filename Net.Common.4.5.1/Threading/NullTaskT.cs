using System;
using System.Threading.Tasks;
using Common.Annotations;
using Net.Common.Functionals;

namespace Net.Common.Threading
{
	[PublicAPI]
	public static class NullTask<T>
	{
		private static readonly Func<Type, Exception, Task<T>> _createNullChildTaskWithException =
			new Func<Type, Exception, Task<T>>(
				(_, exception) =>
				{
					var tcs = new TaskCompletionSource<T>(TaskCreationOptions.AttachedToParent);
					tcs.SetException(exception);
					return tcs.Task;
				})
				.MemoizeConcurrent();

		public static Task<T> ChildInstanceWithException<TException>(TException exception) where TException : Exception
		{
			return _createNullChildTaskWithException(typeof (T), exception);
		}

		public static Task<T> Instance()
		{
			return InstanceWithResult(default(T));
		}

		public static Task<T> InstanceWithResult(T value)
		{
			var tcs = new TaskCompletionSource<T>();
			tcs.SetResult(value);
			return tcs.Task;
		}
	}
}