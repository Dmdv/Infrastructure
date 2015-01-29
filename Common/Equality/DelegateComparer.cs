using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Common.Equality
{
	public class DelegateComparer<T, TIdentity> : IEqualityComparer<T> where T : class
	{
		public DelegateComparer(params Func<T, TIdentity>[] identitySelectors)
		{
			_identitySelectors = identitySelectors;
		}

		public bool Equals(T x, T y)
		{
			return
				_identitySelectors
					.Aggregate(true, (current, identitySelector) => current | Object.Equals(identitySelector(x), identitySelector(y)));
		}

		public int GetHashCode(T obj)
		{
			return
				_identitySelectors
					.Select(identitySelector => identitySelector(obj))
					.Aggregate(0, (current, identity) => current ^ GetIdentityHash(identity));
		}

		private static int GetIdentityHash(TIdentity identity)
		{
			return
				identity is ValueType
					? identity.GetHashCode()
					: (ReferenceEquals(identity, null) ? 0 : identity.GetHashCode());
		}

		private readonly Func<T, TIdentity>[] _identitySelectors;
	}
}