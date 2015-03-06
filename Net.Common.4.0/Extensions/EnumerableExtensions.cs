using System;
using System.Collections.Generic;
using System.Linq;
using Common.Annotations;
using Net.Common.Contracts;
using Net.Common.Functionals;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace Net.Common.Extensions
{
	/// <summary>
	/// This class contains a set of helper methods to facilitate some common operations on enumerables.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Converts source collection of strings to string by concatenating all its elements with some separator.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static string JoinToString(this IEnumerable<string> source, object separator)
		{
			return String.Join(separator.ToString(), source as string[] ?? source.ToArray());
		}

		/// <summary>
		/// Converts source collection of type T to string by concatenating all its elements with some separator.
		/// Each element will be converted to string.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static string JoinToString<T>(this IEnumerable<T> source, object separator)
		{
			return source.Select(item => item.ToString()).JoinToString(separator);
		}

		public static IEnumerable<T> SkipEmptyOrNull<T>([CanBeNull] this IEnumerable<T> values) where T : class
		{
			return values.WhereNotEmptyOrNull(Actions.DoNothing);
		}

		public static IEnumerable<T> WhereNotEmptyOrNull<T>(
			[CanBeNull] this IEnumerable<T> values,
			Action doActionIfEmptyOrNull) where T : class
		{
			if (values == null || !values.Any())
			{
				doActionIfEmptyOrNull();
				return Enumerable.Empty<T>();
			}

			return values.Where(
				item =>
				{
					if (item != null)
					{
						return true;
					}

					doActionIfEmptyOrNull();

					return false;
				});
		}

		/// <summary>
		/// Creates new collection from parameters.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="values"></param>
		/// <returns></returns>
		public static IEnumerable<T> Yield<T>(params T[] values)
		{
			return values;
		}

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

		private static void CheckAssignComparer<T>(ref EqualityComparer<T> comparer)
		{
			if (comparer == null)
			{
				comparer = EqualityComparer<T>.Default;
			}
		}
	}
}