using System.Collections.Generic;
using System.IO;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Extensions;

namespace Net.WindsorCommon.Extensions
{
	public static class WindsorContainerExtensions
	{
		//public static IWindsorContainer RegisterFromAssembliesInDirectories<TBaseInterface>(this IWindsorContainer container, IEnumerable<string> subDirectories)
		//{
		//	subDirectories.ForEach(handlersDirectory => container.RegisterFromAssembliesInDirectories<TBaseInterface>(handlersDirectory));
		//	return container;
		//}

		//public static IWindsorContainer RegisterFromAssembliesInDirectories<TBaseInterface>(this IWindsorContainer container, string directory)
		//{
		//	var targetDirectory = directory;
		//	if (!Directory.Exists(targetDirectory))
		//	{
		//		var programDirectory = PathExtensions.GetProgramDirectoryFullPath();
		//		targetDirectory = Path.Combine(programDirectory, directory);
		//	}

		//	return
		//		container.Register(
		//			Classes.
		//				FromAssemblyInDirectory(new AssemblyFilter(targetDirectory, "*.dll"))
		//				.BasedOn<TBaseInterface>()
		//				.WithService
		//				.FromInterface()
		//				.LifestyleTransient());
		//}
	}
}
