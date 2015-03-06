using System.Threading;

namespace Net.Common.Threading
{
	[Concurrent]
	public sealed class CancellationTokenSourceProvider : ICancellationTokenSourceProvider
	{
		public CancellationTokenSourceProvider()
		{
			_cancellationTokenSource = new CancellationTokenSource();
		}

		public CancellationTokenSource CancellationTokenSource
		{
			get { return _cancellationTokenSource; }
		}

		public void Dispose()
		{
			_cancellationTokenSource.Dispose();
		}

		private readonly CancellationTokenSource _cancellationTokenSource;
	}
}