using System;
using System.Linq;
using Net.Common.Extensions;
using Net.Common.Functionals;
using NLog;

namespace Net.Logging
{
	internal static class LogLevelHelper
	{
		/// <summary>
		/// Returns default log level for event type.
		/// Returned values are memoized, speeding up the resolving.
		/// </summary>
		/// <typeparam name="TEventType"></typeparam>
		/// <param name="eventType"></param>
		/// <returns></returns>

		public static LogLevel GetLogLevel<TEventType>(TEventType eventType) where TEventType : struct, IFormattable
		{
			return InternalGetLogLevel(typeof(TEventType), eventType.ToStringWithInvariantCulture());
		}

		private static readonly Func<Type, string, LogLevel> InternalGetLogLevel =
			new Func<Type, string, LogLevel>(
				(eventType, eventName) =>
				{
					var attributes = eventType
						.GetField(eventName)
						.GetCustomAttributes(typeof(LoggingLevelAttribute), false);

					return attributes.Length == 0 ? LogLevel.Info : ((LoggingLevelAttribute)attributes.Single()).Level;
				}).MemoizeConcurrent();
	}
}
