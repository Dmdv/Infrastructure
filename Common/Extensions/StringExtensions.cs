using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Common.Annotations;
using Common.Contracts;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace Common.Extensions
{
	public static class StringExtensions
	{
		private static readonly Tuple<char, char>[] SymbolsToReplace =
		{
			Tuple.Create('-', '+'),
			Tuple.Create('_', '/')
		};

		public static byte[] Base64UrlDecode(this string arg)
		{
			var copy = (string) arg.Clone();
			// 62nd & 63rd chars of encoding
			copy = SymbolsToReplace.Aggregate(copy, (current, tuple) => current.Replace(tuple.Item1, tuple.Item2));

			// Pad with trailing '='s
			switch (copy.Length%4)
			{
				case 0:
					break; // No pad chars in this case
				case 2:
					copy += "==";
					break; // Two pad chars
				case 3:
					copy += "=";
					break; // One pad char
				default:
					throw new ArgumentException("Bad or corrupted Base64Url string!");
			}

			return Convert.FromBase64String(copy); // Standard base64 decoder
		}

		/// <summary>
		/// Encode an array by Base64 and convert for Url usage
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public static string Base64UrlEncode(this byte[] arg)
		{
			var copy = Convert.ToBase64String(arg);
			// Remove any trailing '='s
			copy = copy.TrimEnd('=');

			// replace 62nd & 64rd chars of encoding
			return SymbolsToReplace.Aggregate(copy, (current, tuple) => current.Replace(tuple.Item2, tuple.Item1));
		}

		[StringFormatMethod("format")]
		public static string FormatString([NotNull] this string format, params object[] values)
		{
			return String.Format(CultureInfo.InvariantCulture, format, values);
		}

		public static string FromBase64(this string value)
		{
			Guard.CheckContainsText(value, "value");
			return Encoding.UTF8.GetString(Convert.FromBase64String(value));
		}

		[PublicAPI]
		public static string IfEmpty(
			[CanBeNull] this string instance,
			string defaultValue = default(string))
		{
			return String.IsNullOrEmpty(instance) || String.IsNullOrWhiteSpace(instance) ? defaultValue : instance;
		}

		public static bool IsEqualAndNotEmpty(this string left, string right)
		{
			return !String.IsNullOrEmpty(left) && left == right;
		}

		public static bool IsNullOrEmpty([CanBeNull] this string target)
		{
			return String.IsNullOrEmpty(target);
		}

		public static bool NotEmptyStringEqualsCi(this string left, string right)
		{
			return !String.IsNullOrEmpty(left) && left.OrdinalEqualsCi(right);
		}

		[UsedImplicitly]
		public static bool OrdinalEquals([CanBeNull] this string left, [CanBeNull] string right, bool ignoreCase)
		{
			if (ReferenceEquals(left, right))
			{
				return true;
			}

			if ((left == null) || (right == null))
			{
				return false;
			}

			var comparisonType = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
			return left.Equals(right, comparisonType);
		}

		/// <summary>
		/// case insensitive
		/// </summary>
		/// <param name = "left"></param>
		/// <param name = "right"></param>
		/// <returns></returns>
		public static bool OrdinalEqualsCi([CanBeNull] this string left, [CanBeNull] string right)
		{
			return left.OrdinalEquals(right, true);
		}

		/// <summary>
		/// case sensitive
		/// </summary>
		/// <param name = "left"></param>
		/// <param name = "right"></param>
		/// <returns></returns>
		public static bool OrdinalEqualsCs([CanBeNull] this string left, [CanBeNull] string right)
		{
			return left.OrdinalEquals(right, false);
		}

		//public static TType DeserializeFromJson<TType>(this string data)
		//{
		//	Guard.CheckContainsText(data, "data");
		//	return JsonConvert.DeserializeObject<TType>(data);
		//}
	}
}