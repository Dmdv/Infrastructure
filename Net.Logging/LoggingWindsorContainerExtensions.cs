using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Net.Common.Patterns;
using Net.Logging.Converters;

namespace Net.Logging
{
	public static class LoggingWindsorContainerExtensions
	{
		/// <summary>
		/// Initializes <paramref name="container"/> with logging interceptor.
		/// </summary>
		/// <param name="container"></param>
		/// <param name="loggingComponentId"></param>
		/// <returns></returns>
		public static IWindsorContainer RegisterLoggingInterceptor(
			this IWindsorContainer container,
			LoggingComponentId loggingComponentId)
		{
			container
				.Register(
					Component
						.For<IComponentLogger<LoggingInterceptorLogEventType>>()
						.Instance(new ComponentLogger<LoggingInterceptorLogEventType>(loggingComponentId)),
					Component
						.For<IHandler<object, string>>()
						.Instance(
							new FirstHandlingAggregatingHandler<object, string>(
								new StreamToStringConverter(),
								new EnumerableToStringConverter(),
								new ExceptionToStringConverter(),
								new TrivialObjectToStringConverter())),
					Component
						.For<LoggingInterceptor>()
						.LifestyleSingleton());

			return container;
		}
	}
}
