using System;

namespace Net.Aspects
{
	[Serializable]
	public sealed class ParameterDescriptor
	{
		public int Index { get; set; }

		public string Name { get; set; }
	}
}
