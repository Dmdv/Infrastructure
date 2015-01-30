using System;
using System.Collections.Generic;
using System.IO;
using Castle.Core;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Annotations;
using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.WindsorCommon.Extensions
{
	public static class Windsor
	{
		[PublicAPI]
		public const LifestyleType DefaultLifestyleType = LifestyleType.Transient;

		[PublicAPI]
		public static BasedOnDescriptor ConfigureInNamespaceAs<TInterface>(
			this IWindsorContainer container,
			LifestyleType lifestyle = DefaultLifestyleType)
			where TInterface : class
		{
			Guard.CheckNotNull(container, "container");

			return
				CreateDesciptorFromAllClassesInNamespace<TInterface>()
					.Configure(x => x.AddDescriptor(new LifestyleDescriptor<TInterface>(lifestyle)));
		}

		[PublicAPI]
		public static bool IsRegistered<TInterface>(this IWindsorContainer container) where TInterface : class
		{
			Guard.CheckNotNull(container, "container");
			return container.Kernel.HasComponent(typeof (TInterface));
		}

		[PublicAPI]
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

		[PublicAPI]
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

		[PublicAPI]
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

		[PublicAPI]
		public static IWindsorContainer RegisterFromAssembliesInDirectories<TBaseInterface>(
			this IWindsorContainer container,
			IEnumerable<string> subDirectories)
		{
			subDirectories.ForEach(
				handlersDirectory => container.RegisterFromAssembliesInDirectories<TBaseInterface>(handlersDirectory));
			return container;
		}

		[PublicAPI]
		public static IWindsorContainer RegisterFromAssembliesInDirectories<TBaseInterface>(
			this IWindsorContainer container,
			string directory)
		{
			var targetDirectory = directory;
			if (!Directory.Exists(targetDirectory))
			{
				var programDirectory = PathExtensions.GetProgramDirectoryFullPath();
				targetDirectory = Path.Combine(programDirectory, directory);
			}

			return
				container.Register(
					Classes.
						FromAssemblyInDirectory(new AssemblyFilter(targetDirectory, "*.dll"))
						.BasedOn<TBaseInterface>()
						.WithService
						.FromInterface()
						.LifestyleTransient());
		}

		[PublicAPI]
		public static void RegisterInNamespaceAs<TInterface>(
			this IWindsorContainer container,
			LifestyleType lifestyle = DefaultLifestyleType) where TInterface : class
		{
			Guard.CheckNotNull(container, "container");

			var baseOnDescriptor = ConfigureInNamespaceAs<TInterface>(container, lifestyle);
			container.Register(baseOnDescriptor);
		}

		[PublicAPI]
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

		[PublicAPI]
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

		[PublicAPI]
		private static ComponentRegistration<TImplementation> CreateComponent<TImplementation>(LifestyleType lifestyle)
			where TImplementation : class
		{
			return Component
				.For<TImplementation>()
				.AddDescriptor(new LifestyleDescriptor<TImplementation>(lifestyle));
		}

		[PublicAPI]
		private static ComponentRegistration<TInterface> CreateComponent<TInterface, TImplementation>(LifestyleType lifestyle)
			where TImplementation : class, TInterface
			where TInterface : class
		{
			return Component
				.For<TInterface>()
				.ImplementedBy<TImplementation>()
				.AddDescriptor(new LifestyleDescriptor<TInterface>(lifestyle));
		}

		// 1. From current assembly.
		// 2. Non public types.
		// 3. The same namespace.
		// 4. All interfaces and self.
		private static BasedOnDescriptor CreateDesciptorFromAllClassesInNamespace<TInterface>()
			where TInterface : class
		{
			return Classes
				.FromAssemblyContaining<TInterface>()
				.IncludeNonPublicTypes()
				.InSameNamespaceAs<TInterface>()
				.WithService.AllInterfaces()
				.WithService.Self();
		}
	}
}