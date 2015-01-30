using System;
using System.Collections.Generic;
using System.Linq;
using Common.Annotations;
using Net.Common.Contracts;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace Net.Common.Extensions
{
	public static class ArrayExtensions
	{
		[Pure]
		public static T[] Add<T>(this T[] array, T addingItem)
		{
			if (array == null)
			{
				return new[]
				{
					addingItem
				};
			}

			var result = array;

			Array.Resize(ref result, array.Length + 1);
			result[array.Length] = addingItem;

			return result;
		}

		[Pure]
		public static T[] Empty<T>()
		{
			return EmptyArray<T>.Instance;
		}

		[Pure]
		public static T[] PushAndCrop<T>(this T[] array, T addingItem, int maxSize)
			where T : class
		{
			Guard.CheckTrue(maxSize > 0, "maxSize must be greater than 0.");

			if (array.IsNullOrEmpty())
			{
				return addingItem.YieldArray(true);
			}

			var list = array.ToList();
			list.Insert(0, addingItem);

			if (list.Count > maxSize)
			{
				list.RemoveRange(maxSize, list.Count - maxSize);
			}

			return list.ToArray();
		}

		[Pure]
		public static T[] Remove<T>(this T[] array, T removingItem)
		{
			Guard.CheckNotEmpty(array, "Unable to remove array item, array is empty.");

			var list = array.ToList();
			Guard.CheckTrue(
				list.Remove(removingItem),
				() => new InvalidOperationException("Unable to remove array item, item doesn't exists."));

			return list.ToArray();
		}

		[Pure]
		public static T[] YieldArray<T>(this T item)
		{
			return new[]
			{
				item
			};
		}

		[Pure]
		public static T[] YieldArray<T>(this T item, bool returnEmptyIfNull)
			where T : class
		{
			if (returnEmptyIfNull && item == null)
			{
				return Empty<T>();
			}

			return new[]
			{
				item
			};
		}

		private static class EmptyArray<T>
		{
			internal static readonly T[] Instance = new T[0];
		}
	}
}