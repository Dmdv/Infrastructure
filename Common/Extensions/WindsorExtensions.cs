using System;
using Castle.Core;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Contracts;

namespace Common.Extensions
{
	public static class WindsorExtensions
	{
		private const LifestyleType DefaultLifestyleType = LifestyleType.Transient;

		public static bool IsRegistered<TInterface>(this IWindsorContainer container) where TInterface : class
		{
			Guard.CheckNotNull(container, "container");
			return container.Kernel.HasComponent(typeof (TInterface));
		}

		public static void Register<TInterface, TImplementation>(
			this IWindsorContainer container,
			LifestyleType lifestyle = DefaultLifestyleType,
			Func<TImplementation> factoryMethod = null)
			where TImplementation : class, TInterface
			where TInterface : class
		{
			Guard.CheckNotNull(container, "container");

			var component = CreateComponent<TInterface, TImplementation>(lifestyle);

			if (factoryMethod != null)
			{
				component.UsingFactoryMethod(factoryMethod);
			}

			container.Register(component);
		}

		public static void Register<TImplementation>(
			this IWindsorContainer container,
			LifestyleType lifestyle = DefaultLifestyleType,
			Func<TImplementation> factoryMethod = null) where TImplementation : class
		{
			Guard.CheckNotNull(container, "container");

			var component = CreateComponent<TImplementation>(lifestyle);

			if (factoryMethod != null)
			{
				component.UsingFactoryMethod(factoryMethod);
			}

			container.Register(component);
		}

		public static void RegisterDescendantsOf<TInterface>(
			this IWindsorContainer container,
			FromAssemblyDescriptor assemblyDescriptor,
			LifestyleType lifestyle = DefaultLifestyleType) where TInterface : class
		{
			Guard.CheckNotNull(container, "container");

			var basedOnDescriptor = assemblyDescriptor
				.IncludeNonPublicTypes()
				.BasedOn<TInterface>();

			switch (lifestyle)
			{
				case LifestyleType.Transient:
					basedOnDescriptor.LifestyleTransient();
					break;
				case LifestyleType.Singleton:
					basedOnDescriptor.LifestyleSingleton();
					break;
				default:
					throw new NotSupportedException("'{0}' lifestyle is not supported.".FormatString(lifestyle));
			}

			container.Register(basedOnDescriptor);
		}

		public static void RegisterInNamespaceAs<TInterface>(
			this IWindsorContainer container,
			LifestyleType lifestyle = DefaultLifestyleType) where TInterface : class
		{
			Guard.CheckNotNull(container, "container");

			container.Register(
				Classes.FromAssemblyInThisApplication()
					.IncludeNonPublicTypes()
					.InSameNamespaceAs<TInterface>()
					.Configure(x => x.AddDescriptor(new LifestyleDescriptor<TInterface>(lifestyle))));
		}

		public static void RegisterInstance<TInterface>(this IWindsorContainer container, TInterface instance)
			where TInterface : class
		{
			Guard.CheckNotNull(container, "container");
			Guard.CheckNotNull(instance, "instance");

			container.Register(
				Component
					.For<TInterface>()
					.Instance(instance));
		}

		public static void RegisterInterfacesFromAssembly<TInterface>(
			this IWindsorContainer container,
			FromAssemblyDescriptor assemblyDescriptor = null,
			LifestyleType lifestyle = DefaultLifestyleType) where TInterface : class
		{
			Guard.CheckNotNull(container, "container");

			if (assemblyDescriptor == null)
			{
				assemblyDescriptor = Classes.FromThisAssembly();
			}

			container.Register(assemblyDescriptor
				.BasedOn<TInterface>()
				.WithService.AllInterfaces()
				.WithService.Self()
				.Configure(x => x.AddDescriptor(new LifestyleDescriptor<TInterface>(lifestyle))));
		}

		private static ComponentRegistration<TImplementation> CreateComponent<TImplementation>(LifestyleType lifestyle)
			where TImplementation : class
		{
			return Component
				.For<TImplementation>()
				.AddDescriptor(new LifestyleDescriptor<TImplementation>(lifestyle));
		}

		private static ComponentRegistration<TInterface> CreateComponent<TInterface, TImplementation>(LifestyleType lifestyle)
			where TImplementation : class, TInterface
			where TInterface : class
		{
			return Component
				.For<TInterface>()
				.ImplementedBy<TImplementation>()
				.AddDescriptor(new LifestyleDescriptor<TInterface>(lifestyle));
		}
	}
}