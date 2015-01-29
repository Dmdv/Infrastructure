using System;
using System.Linq;
using Castle.DynamicProxy;
using Net.Common.Extensions;
using Net.Common.Patterns;
using Net.Common.Threading;

namespace Net.Logging
{
	/// <summary>
	/// Intercepts all invocations of target service methods, adding logging and benchmarking.
	/// <see cref="IComponentLogger{T}"/> of <see cref="LoggingInterceptorLogEventType"/> will be used for logging.
	/// </summary>
	[Concurrent]
	public class LoggingInterceptor : IInterceptor
	{
		public LoggingInterceptor(
			IComponentLogger<LoggingInterceptorLogEventType> logger, 
			IHandler<object, string> objectToStringConverter)
		{
			_logger = logger;
			_objectToStringConverter = objectToStringConverter;
		}

		public void Intercept(IInvocation invocation)
		{
			var executionTimer = new ExecutionTimer();

			LogMethodInvocationEnter(invocation);

			try
			{
				executionTimer.MeasuredExecute(invocation.Proceed);

				LogMethodInvocationResult(invocation, invocation.ReturnValue);
			}
			catch (Exception exception)
			{
				LogMethodInvocationException(exception);

				throw;
			}
			finally
			{
				LogMethodInvocationReturn(invocation, executionTimer);
			}
		}

		private void LogMethodInvocationEnter(IInvocation invocation)
		{
			_logger.Write(
				LoggingInterceptorLogEventType.MethodInvocation,
				"{0} ({1}) enter: {2}",
				GetInvocationName(invocation),
				invocation.Method.ReflectedType.GetInterfaces().Select(iface => iface.Name).JoinToString(", "),
				GetInputParametersString(invocation));
		}

		private void LogMethodInvocationResult(IInvocation invocation, object result)
		{
			if (invocation.Method.ReturnType != typeof(void))
			{
				_logger.Write(
						LoggingInterceptorLogEventType.MethodInvocation,
						"{0} returned: {1}",
						GetInvocationName(invocation),
						_objectToStringConverter.Handle(result));
			}
		}

		private void LogMethodInvocationReturn(IInvocation invocation, ExecutionTimer executionTimer)
		{
			_logger.Write(
				LoggingInterceptorLogEventType.MethodInvocation,
				"{0}: leave ({1:F0} ms)",
				GetInvocationName(invocation),
				executionTimer.ElapsedTime.TotalMilliseconds);
		}

		private void LogMethodInvocationException(Exception exception)
		{
			_logger.Write(LoggingInterceptorLogEventType.MethodInvocationException, _objectToStringConverter.Handle(exception));
		}

		private string GetInputParametersString(IInvocation invocation)
		{
			return (invocation.Arguments ?? Enumerable.Empty<object>()).Select(
				parameterValue => _objectToStringConverter.Handle(parameterValue)).JoinToString(", ");
		}

		private static string GetInvocationName(IInvocation invocation)
		{
			return "method {0}.{1}".FormatString(GetReadableTypeName(invocation.Method.ReflectedType), invocation.Method.Name);
		}

		private static string GetReadableTypeName(Type type)
		{
			return type.IsGenericType
						? type.Name.Split('`').First() + "<" + type.GetGenericArguments().Select(GetReadableTypeName).JoinToString(",")
						: type.Name;
		}

		private readonly IComponentLogger<LoggingInterceptorLogEventType> _logger;
		private readonly IHandler<object, string> _objectToStringConverter;
	}
}
