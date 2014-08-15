using System;
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
	public static class CastExtensions
	{
		[CanBeNull]
		public static TTo As<TTo>([CanBeNull] this object item) where TTo : class
		{
			return item as TTo;
		}

		[NotNull]
		public static TTo Cast<TTo>([NotNull] this object item)
		{
			Guard.CheckNotNull(item, "item");

			try
			{
				return (TTo) item;
			}
			catch (InvalidCastException)
			{
				throw new InvalidCastException(string.Format("Invalid cast from '{0}' -> '{1}'.", item.GetType(), typeof (TTo)));
			}
		}

		public static TTo SafeCast<TTo>(this object instance) where TTo : class
		{
			return SafeCast<TTo>(
				instance,
				"Invalid cast from type {0} to type {1}".FormatString(instance.GetType().FullName, typeof (TTo).FullName));
		}

		public static TTo SafeCast<TTo>(this object instance, string message) where TTo : class
		{
			return SafeCast<TTo>(instance, new InvalidCastException(message));
		}

		public static TTo SafeCast<TTo>(this object instance, Exception invalidCastException) where TTo : class
		{
			var cast = instance as TTo;
			if (cast != null)
			{
				return cast;
			}

			throw invalidCastException;
		}
	}
}