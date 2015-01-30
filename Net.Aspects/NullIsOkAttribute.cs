using System;

namespace Net.Aspects
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = true)]
	public sealed class NullIsOkAttribute : Attribute
	{
	}
}
