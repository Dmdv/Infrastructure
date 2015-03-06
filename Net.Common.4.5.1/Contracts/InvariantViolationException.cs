using System;
using System.Runtime.Serialization;

// ReSharper disable CodeCleanup
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Global

namespace Net.Common.Contracts
{
	[Serializable]
	public class InvariantViolationException : Exception
	{
		public InvariantViolationException()
		{
		}

		public InvariantViolationException(string message)
			: base(message)
		{
		}

		public InvariantViolationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected InvariantViolationException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}
