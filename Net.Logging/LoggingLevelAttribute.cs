using System;
using NLog;

namespace Net.Logging
{
	/// <summary>
	/// Should be used to declare the default logging level of some log event type
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class LoggingLevelAttribute : Attribute
	{
		public LoggingLevelAttribute(Level level)
		{
			_level = level;
		}

		public LogLevel Level 
		{ 
			get
			{
				switch (_level)
				{
					case Logging.Level.Error:
						return LogLevel.Error;
					case Logging.Level.Warning:
						return LogLevel.Warn;
					case Logging.Level.Info:
						return LogLevel.Info;
					case Logging.Level.Debug:
						return LogLevel.Debug;
					default:
						throw new InvalidEnumValueException(_level);
				}
			}
		}

		private readonly Level _level;
	}
}
