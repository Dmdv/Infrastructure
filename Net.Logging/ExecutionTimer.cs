using System;
using System.Diagnostics;

namespace Net.Logging
{
	public sealed class ExecutionTimer
	{
		public TimeSpan ElapsedTime { get; private set; }

		public T MeasuredExecute<T>(Func<T> invocation)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			try
			{
				return invocation();
			}
			finally
			{
				stopwatch.Stop();
				ElapsedTime += stopwatch.Elapsed;
			}
		}

		public void MeasuredExecute(Action invocation)
		{
			MeasuredExecute<object>(
				() =>
				{
					invocation();
					return null;
				});
		}
	}
}
