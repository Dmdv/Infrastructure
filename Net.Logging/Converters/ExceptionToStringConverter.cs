using System;
using System.Text;
using Net.Common.Patterns;
using Net.Common.Threading;

namespace Net.Logging.Converters
{
	[Concurrent]
	public class ExceptionToStringConverter : DelegatingHandler<object, string>
	{
		public ExceptionToStringConverter()
			: base(
				value => value is Exception,
				value =>
					{
						var exception = (Exception)value;
						var stringBuilder = new StringBuilder("Exception thrown: ");
						Action<Exception> appendException = e =>
						{
						    stringBuilder.AppendLine(e.Message);
						    stringBuilder.AppendLine("Call stack:");
						    stringBuilder.AppendLine(e.StackTrace);
						};

						appendException(exception);
						while (exception.InnerException != null)
						{
							stringBuilder.AppendLine("Inner exception: ");
							appendException(exception.InnerException);
							exception = exception.InnerException;
						}

						if (exception.InnerException != null)
						{
							stringBuilder.AppendLine("There is at least one more inner exception.");
						}

						return stringBuilder.ToString();
					})
		{
		}
	}
}
