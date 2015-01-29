using System.IO;
using Net.Common.Extensions;
using Net.Common.Patterns;
using Net.Common.Threading;

namespace Net.Logging.Converters
{
	[Concurrent]
	public class StreamToStringConverter : DelegatingHandler<object, string>
	{
		public StreamToStringConverter()
			: base(
				value => value is Stream,
				value =>
				{
					var stream = (Stream)value;
					return "(Stream: Length={0}, Position={1})".FormatString(stream.Length, stream.Position);
				})
		{
		}
	}
}
