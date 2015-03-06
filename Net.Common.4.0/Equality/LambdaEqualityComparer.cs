using System;
using Common.Annotations;

namespace Net.Common.Equality
{
	public static class LambdaEqualityComparer
	{
		[PublicAPI]
		public static LambdaEqualityComparer<TObj> Create<TObj>(
			TObj typedInstance,
			Func<TObj, TObj, bool> comparer) where TObj : class
			// ReSharper restore UnusedParameter.Global
		{
			return new LambdaEqualityComparer<TObj>(comparer);
		}
	}
}