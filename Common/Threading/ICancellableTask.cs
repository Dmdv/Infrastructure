using System;

namespace Net.Common.Threading
{
	internal interface ICancellableTask : IDisposable
	{
		void CancelAndStop();
	}
}
