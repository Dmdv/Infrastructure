using System;

namespace Net.Common.Threading
{
	/// <summary>
	/// Declares that some class or interface is supposed to be used in the concurrent environment.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
	public sealed class ConcurrentAttribute : Attribute
	{
	}
}
