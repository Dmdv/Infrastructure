using System;
using System.Collections.Generic;

namespace Net.Common.Equality
{
	public class LambdaEqualityComparer<TObj> : IEqualityComparer<TObj> where TObj : class
	{
		public LambdaEqualityComparer(Func<TObj, TObj, bool> comparer)
		{
			_comparer = comparer;
		}

		public bool Equals(TObj x, TObj y)
		{
			if (ReferenceEquals(x, y))
			{
				return true;
			}

			if ((x == null) || (y == null))
			{
				return false;
			}

			return _comparer(x, y);
		}

		public int GetHashCode(TObj obj)
		{
			return obj.GetHashCode();
		}

		private readonly Func<TObj, TObj, bool> _comparer;
	}
}