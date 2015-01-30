using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Common.Annotations;
using Net.Common.Contracts;
using Net.Common.Threading;

namespace Net.Common.Extensions
{
	[PublicAPI]
	[Concurrent]
	public static class PathExtensions
	{
		public static string GetProgramDirectoryFullPath()
		{
			return Path.GetFullPath(GetProgramDirectoryName());
		}

		[PublicAPI]
		public static string GetProgramDirectoryName()
		{
			return GetAssemblyDirectoryName(GetGuardedEntryAssembly());
		}

		public static string GetProgramName()
		{
			return Path.GetFileName(GetGuardedEntryAssembly().Location);
		}

		public static IEnumerable<string> GetProgramSubDirectories()
		{
			return GetPrivateBinPath().Split(new[]
			{
				';'
			},
				StringSplitOptions.RemoveEmptyEntries);
		}

		private static string GetAssemblyDirectoryName(Assembly assembly)
		{
			var directoryName = Path.GetDirectoryName(assembly.Location);

			Guard.CheckNotNull(directoryName, "directoryName");

			// ReSharper disable once PossibleNullReferenceException
			var result = directoryName.Trim();

			Guard.CheckInvariant(!string.IsNullOrEmpty(result), "Assembly directory name cannot be empty.");
			if (result.Last() != Path.DirectorySeparatorChar)
			{
				result += Path.DirectorySeparatorChar;
			}

			return result;
		}

		private static Assembly GetGuardedEntryAssembly()
		{
			var entryAssembly = Assembly.GetEntryAssembly();
			Guard.CheckNotNullInvariant(entryAssembly, "Entry assembly is null.");
			return entryAssembly;
		}

		private static string GetPrivateBinPath()
		{
			if (AppDomain.CurrentDomain.SetupInformation.PrivateBinPath != null)
			{
				return AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
			}

			var configFile = XElement.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
			var probingElement = (from runtime in configFile.Descendants("runtime")
			                      from assemblyBinding in
				                      runtime.Elements(XName.Get("assemblyBinding", "urn:schemas-microsoft-com:asm.v1"))
			                      from probing in
				                      assemblyBinding.Elements(XName.Get("probing", "urn:schemas-microsoft-com:asm.v1"))
			                      select probing)
				.FirstOrDefault();

			if (probingElement != null)
			{
				var attribute = probingElement.Attribute("privatePath");
				if (attribute != null)
				{
					return attribute.Value;
				}
			}

			return string.Empty;
		}
	}
}