namespace Net.Logging
{
	/// <summary>
	/// This class exists for removing reference on NLog.dll in client classes.
	/// </summary>
	public class ComputerNameProviderApplier
	{
		public ComputerNameProviderApplier(IComputerNameProvider computerNameProvider)
		{
			_computerNameProvider = computerNameProvider;
		}

		public void Apply()
		{
			ComputerNameLayoutRenderer.SetComputerNameProvider(_computerNameProvider);
		}

		private readonly IComputerNameProvider _computerNameProvider;
	}
}