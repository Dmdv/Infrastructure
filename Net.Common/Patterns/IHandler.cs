namespace Net.Common.Patterns
{
	public interface IHandler<in TValue, out TResult>
	{
		bool CanHandle(TValue value);

		TResult Handle(TValue value);
	}
}
