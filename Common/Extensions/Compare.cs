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
	/// Класс используется для выборки элементов коллекции, критерии уникальности которых определяеются по заданным полям.
	/// </summary>
	public static class Compare
	{
		//public static IEnumerable<TSource> DistinctBy<TSource>(this IEnumerable<TSource> source, params Func<TSource, object>[] identitySelectors) 
		//	where TSource : class
		//{
		//	Guard.CheckNotNull(identitySelectors, "identitySelectors");
		//	return source.Distinct(By(identitySelectors));
		//}

		//public static IEqualityComparer<TSource> By<TSource>(params Func<TSource, object>[] identitySelectors) 
		//	where TSource : class
		//{
		//	Guard.CheckNotNull(identitySelectors, "identitySelectors");
		//	return new DelegateComparer<TSource, object>(identitySelectors);
		//}
	}
}