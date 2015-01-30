using System;
using System.Globalization;
using System.Text;
using Common.Annotations;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace Net.Common.Extensions
{
	public static class FormattableExtensions
	{
		public static string ToStringWithInvariantCulture(this IFormattable value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		public static string ToStringWithInvariantCulture(this IFormattable value, string format)
		{
			return value.ToString(format, CultureInfo.InvariantCulture);
		}

		public static string ToStringWithInvariantCulture(this char value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public static string ToStringWithInvariantCulture(this int value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public static string ToStringWithInvariantCulture(this bool value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}
	}
}
