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
	public sealed class ContractViolationException : Exception
	{
		public ContractViolationException()
		{
		}

		public ContractViolationException(string message)
			: base(message)
		{
		}

		public ContractViolationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		private ContractViolationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
