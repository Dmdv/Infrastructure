namespace Net.WindsorCommon.Aspects
{
	public interface IAspectApplier
	{
		T Apply<T>(T service) where T : class;
	}
}