using Common.Annotations;
using Net.Logging;

namespace Net.WindsorCommon.Aspects
{
	[PublicAPI]
	public class LoggingAspectApplier : IAspectApplier
	{
		public LoggingAspectApplier(LoggingInterceptor interceptor)
		{
			_aspectApplier = new AspectApplier(interceptor);
		}

		public T Apply<T>(T service) where T : class
		{
			return _aspectApplier.Apply(service);
		}

		private readonly AspectApplier _aspectApplier;
	}
}