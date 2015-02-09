using System;
using Net.Common.Contracts;

namespace Net.MsAccess.Extensions
{
	internal static class StringExtensions
	{
		public static double ToDouble(this string value)
		{
			Guard.CheckContainsText(value, "value");

			double outValue;

			if (double.TryParse(value, out outValue))
			{
				return outValue;
			}

			throw new ArgumentException(value, string.Format("value = '{0}'", value));
		}

		public static double ToDoubleOrZero(this string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return 0;
			}
			double outValue;
			return double.TryParse(value, out outValue) ? outValue : 0.0d;
		}

		public static Tuple<bool, double> ToDoubleSafe(this string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return new Tuple<bool, double>(false, -1.0d);
			}
			double outValue;
			return
				double.TryParse(value, out outValue)
					? new Tuple<bool, double>(true, outValue)
					: new Tuple<bool, double>(false, -1.0d);
		}

		public static float ToFloat(this string value)
		{
			Guard.CheckContainsText(value, "value");

			float outValue;

			if (float.TryParse(value, out outValue))
			{
				return outValue;
			}

			throw new ArgumentException(value, string.Format("value = '{0}'", value));
		}

		public static float ToFloatOrZero(this string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return 0;
			}
			float outValue;
			return float.TryParse(value, out outValue) ? outValue : 0.0f;
		}

		public static int ToInt(this string value)
		{
			Guard.CheckContainsText(value, "value");

			int outValue;

			if (int.TryParse(value, out outValue))
			{
				return outValue;
			}

			throw new ArgumentException(value, string.Format("value = '{0}'.", value));
		}

		public static int ToIntOrZero(this string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return 0;
			}
			int outValue;
			return int.TryParse(value, out outValue) ? outValue : 0;
		}
	}
}