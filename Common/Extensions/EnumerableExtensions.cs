using System;
using System.Collections.Generic;
using System.Linq;
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

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> container)
		{
			return container == null || !container.Any();
		}

		public static IEnumerable<TLeftContainer> SelectDifferent<TLeftContainer, TRightContainer, TValue>(
			this IEnumerable<TLeftContainer> left,
			IEnumerable<TRightContainer> right,
			Func<TLeftContainer, TValue> leftExtractor,
			Func<TRightContainer, TValue> rightExtractor,
			EqualityComparer<TValue> comparer = null)
		{
			CheckAssignComparer(ref comparer);
			return
				left.Where(leftItem => right.All(rightItem => !comparer.Equals(leftExtractor(leftItem), rightExtractor(rightItem))));
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

		public static IEnumerable<TLeftContainer> SelectSame<TLeftContainer, TRightContainer, TValue>(
			this IEnumerable<TLeftContainer> left,
			IEnumerable<TRightContainer> right,
			Func<TLeftContainer, TValue> leftExtractor,
			Func<TRightContainer, TValue> rightExtractor,
			EqualityComparer<TValue> comparer = null)
		{
			CheckAssignComparer(ref comparer);
			return
				left.Where(leftItem => right.Any(rightItem => comparer.Equals(leftExtractor(leftItem), rightExtractor(rightItem))));
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

		public static bool SequenceEqualSafe(
			this IEnumerable<string> source,
			IEnumerable<string> comparable,
			StringComparison comparisonOption)
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
						if (!comparableEnumerator.MoveNext() ||
						    !string.Equals(sourceEnumerator.Current, comparableEnumerator.Current, comparisonOption))
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

		public static bool SequenceEqualSafe<T>(
			this IEnumerable<T> source,
			IEnumerable<T> comparable,
			IEqualityComparer<T> comparer = null)
		{
			if (ReferenceEquals(source, null) || ReferenceEquals(comparable, null))
			{
				return false;
			}

			return source.SequenceEqual(comparable, comparer);
		}

		public static TSource Single<TSource>(
			[NotNull] this IEnumerable<TSource> source,
			[NotNull] Func<TSource, bool> predicate,
			string moreThanOneElementMessage = null,
			string noElementsMessage = null)
		{
			Guard.CheckNotNull(source, "source");
			Guard.CheckNotNull(predicate, "predicate");

			var result = default(TSource);
			var num = 0;
			foreach (var item in source.Where(predicate))
			{
				if (num > 0)
				{
					throw new InvalidOperationException(moreThanOneElementMessage ?? "Found more than one element.");
				}

				result = item;
				num++;
			}

			if (num == 0)
			{
				throw new InvalidOperationException(noElementsMessage ?? "Not found element that match predicate.");
			}

			return result;
		}

		/// <summary>
		/// Creates new collection with a target object as its sole element.
		/// </summary>
		public static IEnumerable<T> Yield<T>(this T value)
		{
			return new[]
			{
				value
			};
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