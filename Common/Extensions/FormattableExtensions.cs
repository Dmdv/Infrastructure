using System;
using System.Globalization;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace Common.Extensions
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
	}
}
