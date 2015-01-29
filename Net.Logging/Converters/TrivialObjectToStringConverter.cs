using Net.Common.Functionals;
using Net.Common.Patterns;
using Net.Common.Threading;

namespace Net.Logging.Converters
{
	[Concurrent]
	public sealed class TrivialObjectToStringConverter : DelegatingHandler<object, string>
	{
		public TrivialObjectToStringConverter()
			: base(
				Functions.GetTrue,
				value => value != null ? value.ToString() : "<null>")
		{
		}
	}
}
