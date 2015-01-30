using Net.Common.Threading;

namespace Net.Common.Functionals
{
	[Concurrent]
	public static class Actions
	{
		public static void DoNothing()
		{
		}

		public static void DoNothing<T>(T param)
		{
		}

		public static void DoNothing<T1, T2>(T1 param1, T2 param2)
		{
		}

		public static void DoNothing<T1, T2, T3>(T1 param1, T2 param2, T3 param3)
		{
		}
	}
}