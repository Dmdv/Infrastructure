using System;
using System.Collections.Generic;
using System.Linq;
using Net.Common.Contracts;
using Net.Common.Extensions;
using Net.Common.Threading;
using NLog;

namespace Net.Logging
{
	[Concurrent]
	public sealed class ComponentLogger<TEventType> :
		IComponentLogger<TEventType> where TEventType : struct, IFormattable
	{
		public ComponentLogger(LoggingComponentId loggingComponentId)
		{
			ComponentId = loggingComponentId;
			_eventTypePrefix = typeof(TEventType).Name;
			CreateLoggers();
		}

		public LoggingComponentId ComponentId { get; private set; }

		public void Write(TEventType eventId, string formattedMessage, params object[] args)
		{
			Guard.CheckNotNull(formattedMessage, "formattedMessage");
			Guard.CheckNotNull(args, "args");

			_loggers[GetLoggerKey(eventId)]
				.Log(
					LogLevelHelper.GetLogLevel(eventId),
					GetFormattedMessage(eventId, formattedMessage, args));
		}

		public void Write(TEventType eventId, Exception exception, string formattedMessage, params object[] args)
		{
			_loggers[GetLoggerKey(eventId)]
				.Log(
					LogLevelHelper.GetLogLevel(eventId),
					GetFormattedMessage(eventId, formattedMessage, args),
					exception);
		}

		public void Write(TEventType eventId, Exception exception)
		{
			Write(eventId, exception, string.Empty);
		}

		public void Flush()
		{
			LogManager.Flush();
		}

		private void CreateLoggers()
		{
			foreach (var loggerKey in GetLoggerKeysForCurrentEventType())
			{
				_loggers.Add(loggerKey, LogManager.GetLogger(loggerKey));
			}
		}

		private IEnumerable<string> GetLoggerKeysForCurrentEventType()
		{
			return from TEventType eventId in Enum.GetValues(typeof(TEventType)) select GetLoggerKey(eventId);
		}

		private string GetLoggerKey(TEventType eventId)
		{
			return "{0}/{1}.{2}".FormatString(ComponentId.Name, _eventTypePrefix, eventId);
		}

		private string GetFormattedMessage(TEventType eventId, string formattedMessage, params object[] args)
		{
			return "{0}.{1} {2}"
				.FormatString(
					_eventTypePrefix,
					eventId,
					args.Length == 0 ? formattedMessage : formattedMessage.FormatString(args));
		}

		private readonly IDictionary<string, Logger> _loggers = new Dictionary<string, Logger>();
		private readonly string _eventTypePrefix;
	}
}
