using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace Common.Extensions
{
	/// <summary>
	/// This class contains a set of helper methods to facilitate some common operations on enumerables.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Functional equivalent to foreach operator.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="action">Action to be performed for each element of the source collection</param>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var item in source)
			{
				action(item);
			}
		}

		/// <summary>
		/// Creates new collection with a target object as its sole element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static IEnumerable<T> Yield<T>(this T value)
		{
			return new[] { value };
		}

		public static IEnumerable<TLeftContainer> SelectSame<TLeftContainer, TRightContainer, TValue>(
			this IEnumerable<TLeftContainer> left,
			IEnumerable<TRightContainer> right,
			Func<TLeftContainer, TValue> leftExtractor,
			Func<TRightContainer, TValue> rightExtractor,
			EqualityComparer<TValue> comparer = null)
		{
			CheckAssignComparer(ref comparer);
			return left.Where(leftItem => right.Any(rightItem => comparer.Equals(leftExtractor(leftItem), rightExtractor(rightItem))));
		}

		public static IEnumerable<TContainer> SelectSame<TContainer, TValue>(
			this IEnumerable<TContainer> left,
			IEnumerable<TContainer> right,
			Func<TContainer, TValue> extractor,
			EqualityComparer<TValue> comparer = null)
		{
			CheckAssignComparer(ref comparer);
			return left.Where(leftItem => right.Any(rightItem => comparer.Equals(extractor(leftItem), extractor(rightItem))));
		}

		public static IEnumerable<TValue> SelectSame<TValue>(
			this IEnumerable<TValue> left,
			IEnumerable<TValue> right,
			EqualityComparer<TValue> comparer = null)
		{
			CheckAssignComparer(ref comparer);
			return left.Where(leftItem => right.Any(rightItem => comparer.Equals(leftItem, rightItem)));
		}

		public static IEnumerable<TLeftContainer> SelectDifferent<TLeftContainer, TRightContainer, TValue>(
			this IEnumerable<TLeftContainer> left,
			IEnumerable<TRightContainer> right,
			Func<TLeftContainer, TValue> leftExtractor,
			Func<TRightContainer, TValue> rightExtractor,
			EqualityComparer<TValue> comparer = null)
		{
			CheckAssignComparer(ref comparer);
			return left.Where(leftItem => right.All(rightItem => !comparer.Equals(leftExtractor(leftItem), rightExtractor(rightItem))));
		}

		public static IEnumerable<TContainer> SelectDifferent<TContainer, TValue>(
			this IEnumerable<TContainer> left,
			IEnumerable<TContainer> right,
			Func<TContainer, TValue> extractor,
			EqualityComparer<TValue> comparer = null)
		{
			CheckAssignComparer(ref comparer);
			return left.Where(leftItem => right.All(rightItem => !comparer.Equals(extractor(leftItem), extractor(rightItem))));
		}

		public static IEnumerable<TValue> SelectDifferent<TValue>(
			this IEnumerable<TValue> left,
			IEnumerable<TValue> right,
			EqualityComparer<TValue> comparer = null)
		{
			CheckAssignComparer(ref comparer);
			return left.Where(leftItem => right.All(rightItem => !comparer.Equals(leftItem, rightItem)));
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> container)
		{
			return container == null || !container.Any();
		}

		public static bool SequenceEqualSafe(this IEnumerable<string> source, IEnumerable<string> comparable, StringComparison comparisonOption)
		{
			if (ReferenceEquals(source, null) || ReferenceEquals(comparable, null))
			{
				return false;
			}

			using (var sourceEnumerator = source.GetEnumerator())
			{
				using (var comparableEnumerator = comparable.GetEnumerator())
				{
					while (sourceEnumerator.MoveNext())
					{
						if (!comparableEnumerator.MoveNext() || !string.Equals(sourceEnumerator.Current, comparableEnumerator.Current, comparisonOption))
						{
							return false;
						}
					}

					if (comparableEnumerator.MoveNext())
					{
						return false;
					}
				}
			}

			return true;
		}

		public static bool SequenceEqualSafe<T>(this IEnumerable<T> source, IEnumerable<T> comparable, IEqualityComparer<T> comparer = null)
		{
			if (ReferenceEquals(source, null) || ReferenceEquals(comparable, null))
			{
				return false;
			}

			return source.SequenceEqual(comparable, comparer);
		}

		private static void CheckAssignComparer<T>(ref EqualityComparer<T> comparer)
		{
			if (comparer == null)
			{
				comparer = EqualityComparer<T>.Default;
			}
		}
	}
}
