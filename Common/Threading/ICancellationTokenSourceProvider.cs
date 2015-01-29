using System;
using System.Threading;

namespace Net.Common.Threading
{
	public interface ICancellationTokenSourceProvider : IDisposable
	{
		CancellationTokenSource CancellationTokenSource { get; }
	}
}