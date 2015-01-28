using System;

namespace Common.Exceptions
{
	public class InvalidArgumentException : Exception
	{
		public InvalidArgumentException(string message, object value)
			: base(message)
		{
			Value = value;
		}

		public object Value { get; private set; }
	}
}