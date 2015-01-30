namespace Net.Common.Patterns
{
	public interface IHandler<in TValue>
	{
		bool CanHandle(TValue value);

		void Handle(TValue value);
	}
}
