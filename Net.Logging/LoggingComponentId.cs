namespace Net.Logging
{
	/// <summary>
	/// Contains an enumeration of multiple logging components, which are used to tune logging.
	/// </summary>
	public sealed class LoggingComponentId
	{
		public string Name { get; private set; }

		private LoggingComponentId(string name)
		{
			Name = name;
		}
	}
}
