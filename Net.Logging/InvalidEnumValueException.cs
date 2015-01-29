using System;

namespace Net.Logging
{
	public class InvalidEnumValueException : Exception
	{
		public InvalidEnumValueException(Level level)
		{
		}
	}
}