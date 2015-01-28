using System;
using System.Collections.Generic;
using System.Linq;
using Common.Contracts;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace NetCommon.Equality
{
	/// <summary>
	/// Класс используется для выборки элементов коллекции, критерии уникальности которых определяеются по заданным полям.
	/// </summary>
	public static class Compare
	{
		public static IEqualityComparer<TSource> By<TSource>(params Func<TSource, object>[] identitySelectors)
			where TSource : class
		{
			Guard.CheckNotNull(identitySelectors, "identitySelectors");
			return new DelegateComparer<TSource, object>(identitySelectors);
		}

		public static IEnumerable<TSource> DistinctBy<TSource>(
			this IEnumerable<TSource> source,
			params Func<TSource, object>[] identitySelectors)
			where TSource : class
		{
			Guard.CheckNotNull(identitySelectors, "identitySelectors");
			return source.Distinct(By(identitySelectors));
		}
	}
}