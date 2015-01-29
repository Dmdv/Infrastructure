using System;
using System.Collections.Concurrent;
using Common.Annotations;

namespace Net.Common.Functionals
{
	/// <summary>
	/// This helper class contains static methods, which add memoization to target functions.
	/// Memoization is a mechanism of caching function results on a limited set of input parameters values.
	/// </summary>
	public static class MemoizeExtensions
	{
		/// <summary>
		/// Creates a wrapper function, which memoizes the target function results on some limited set of input parameter values.
		/// This method can be used concurrently.
		/// </summary>
		/// <typeparam name="TKey">Key type.</typeparam>
		/// <typeparam name="TResult">Result type.</typeparam>
		/// <param name="func">Target function.</param>
		/// <returns>Wrapped target function</returns>
		[PublicAPI]
		public static Func<TKey, TResult> MemoizeConcurrent<TKey, TResult>(this Func<TKey, TResult> func)
		{
			var cache = new ConcurrentDictionary<TKey, TResult>();

			return key => cache.GetOrAdd(key, func);
		}

		/// <summary>
		/// Creates a wrapper function, which memoizes the target function results on some limited set of input parameters values.
		/// This method can be used concurrently.
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <typeparam name="TResult">Result type.</typeparam>
		/// <param name="func">Target function.</param>
		/// <returns>Wrapped target function.</returns>
		[PublicAPI]
		public static Func<T1, T2, TResult> MemoizeConcurrent<T1, T2, TResult>(this Func<T1, T2, TResult> func)
		{
			var memoizedFunc = new Func<Tuple<T1, T2>, TResult>(tuple => func(tuple.Item1, tuple.Item2)).MemoizeConcurrent();

			return (t1, t2) => memoizedFunc(new Tuple<T1, T2>(t1, t2));
		}
	}
}