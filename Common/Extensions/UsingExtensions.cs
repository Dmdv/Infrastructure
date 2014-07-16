using System;
using Common.Extensions.Monads;

namespace Common.Extensions
{
	public static class UsingExtensions
	{
		public static Action<Action> Disposeable(this IDisposable disposable)
		{
			return disposable.ExecuteAndDispose;
		}

		public static void ExecuteAndDispose(this IDisposable disposable, Action callee)
		{
			disposable.Do(x =>
			{
				using (x)
				{
					callee();
				}
			});
		}

		//public static TResult ExecuteAndDispose<TResult>(this IDisposable disposable, Func<TResult> callee)
		//{
		//	return disposable.DoReturn(x =>
		//	{
		//		using (x)
		//		{
		//			return callee();
		//		}
		//	});
		//}

		public static Action Using(this Action callee, IDisposable disposable)
		{
			return () => disposable.ExecuteAndDispose(callee);
		}
	}
}