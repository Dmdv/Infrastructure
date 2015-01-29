namespace Net.Logging
{
	public enum LoggingInterceptorLogEventType
	{
		[LoggingLevel(Level.Debug)]
		MethodInvocation,
		[LoggingLevel(Level.Error)]
		MethodInvocationException
	}
}
