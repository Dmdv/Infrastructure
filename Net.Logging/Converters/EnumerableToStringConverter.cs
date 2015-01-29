using System.Collections;
using System.Linq;
using Net.Common;
using Net.Common.Extensions;
using Net.Common.Patterns;
using Net.Common.Threading;

namespace Net.Logging.Converters
{
	[Concurrent]
	public class EnumerableToStringConverter : DelegatingHandler<object, string>
	{
		public EnumerableToStringConverter()
			: base(
				value => value is IEnumerable && !(value is string),
				value =>
				{
					var enumerableValue = ((IEnumerable)value).Cast<object>();
					return "Enumerable: Count = {0}, Elements = ( {1} )".FormatString(
							enumerableValue.Count(),
							enumerableValue.JoinToString(", "));
				})
		{
		}
	}
}
