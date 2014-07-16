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
	public static class EnumerableEx
	{
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
	}
}
