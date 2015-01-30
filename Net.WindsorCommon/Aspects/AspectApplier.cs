using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.WindsorCommon.Aspects
{
	/// <summary>
	/// Convenience class, which dynamically applies invocation interceptors <see cref="IInterceptor"/> to some service.
	/// This class extends interception mechanism of Castle.Windsor.
	/// </summary>
	public sealed class AspectApplier : IAspectApplier
	{
		public AspectApplier(IEnumerable<IInterceptor> aspectsToAddToServices)
		{
			var toAddToServices = aspectsToAddToServices.ToArray();
			Guard.CheckElementsNotNull(toAddToServices, @"aspectsToAddToServices must contain at least 1 element.");

			_aspectsToAddToServices = toAddToServices;
		}

		public AspectApplier(IInterceptor aspectToAdd)
			: this(aspectToAdd.Yield())
		{
		}

		/// <summary>
		/// Dynamically applies invocation interceptors <see cref="IInterceptor"/> to some service, by creating a proxy of the target service.
		/// </summary>
		/// <typeparam name="T">Interface of the target service</typeparam>
		/// <param name="service">Instance of the service</param>
		/// <returns>Intercepted service proxy</returns>
		public T Apply<T>(T service) where T : class
		{
			CheckWrapServiceConstraints<T>();

			return (T) new ProxyGenerator().CreateInterfaceProxyWithTargetInterface(
				typeof (T),
				service,
				_aspectsToAddToServices.ToArray());
		}

		private static void CheckWrapServiceConstraints<T>()
		{
			var serviceType = typeof (T);
			Guard.CheckTrue(
				serviceType.IsInterface,
				() => new ArgumentException(
					"Service '{0}' must be an interface.".FormatString(serviceType)));
		}

		private readonly IEnumerable<IInterceptor> _aspectsToAddToServices;
	}
}