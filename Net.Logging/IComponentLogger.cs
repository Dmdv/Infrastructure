using System;
using Net.Common.Threading;

namespace Net.Logging
{
	[Concurrent]
	public interface IComponentLogger<in T> where T : struct
	{
		LoggingComponentId ComponentId { get; }

		void Write(T eventId, string formattedMessage, params object[] args);

		void Write(T eventId, Exception exception, string formattedMessage, params object[] args);

		void Write(T eventId, Exception exception);

		void Flush();
	}
}
