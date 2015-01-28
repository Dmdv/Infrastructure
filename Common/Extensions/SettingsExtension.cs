using System.Collections.Specialized;
using Common.Contracts;

namespace Common.Extensions
{
	public static class SettingsExtension
	{
		public static string ReadConfigValue(this NameValueCollection config, string paramName)
		{
			var configValue = Guard.GetNotNull(config, "config")[paramName];
			Guard.CheckContainsText(configValue, paramName);
			return configValue;
		}
	}
}