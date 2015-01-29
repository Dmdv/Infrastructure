using System;
using Net.Common.Threading;

namespace Net.Logging
{
	[Concurrent]
	internal sealed class DefaultComputerNameProvider : IComputerNameProvider
	{
		public string GetComputerName()
		{
			return Environment.MachineName;
		}
	}
}