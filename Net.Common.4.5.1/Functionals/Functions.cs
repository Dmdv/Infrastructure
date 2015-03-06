using Net.Common.Threading;

namespace Net.Common.Functionals
{
	[Concurrent]
	public sealed class Functions
	{
		public static bool GetFalse()
		{
			return false;
		}

		public static bool GetFalse<T>(T param)
		{
			return false;
		}

		public static bool GetInverted(bool param)
		{
			return !param;
		}

		public static T GetSelf<T>(T param)
		{
			return param;
		}

		public static bool GetTrue()
		{
			return true;
		}

		public static bool GetTrue<T>(T param)
		{
			return true;
		}
	}
}