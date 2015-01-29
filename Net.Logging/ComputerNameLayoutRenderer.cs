using System.Text;
using Net.Common.Contracts;
using Net.Common.Threading;
using NLog;
using NLog.LayoutRenderers;

namespace Net.Logging
{
	[LayoutRenderer("ComputerName")]
	[Concurrent]
	public sealed class ComputerNameLayoutRenderer : LayoutRenderer
	{
		public static void SetComputerNameProvider(IComputerNameProvider computerNameProvider)
		{
			Guard.CheckTrue(
				computerNameProvider.GetType().FullName != typeof(DefaultComputerNameProvider).FullName,
				"Default computer name provider is already used.");

			Guard.CheckTrue(
				_computerNameProvider.GetType().FullName == typeof(DefaultComputerNameProvider).FullName, 
				"Computer name provider is already set.");

			_computerNameProvider = computerNameProvider;
		}

		protected override void Append(StringBuilder builder, LogEventInfo logEvent)
		{
			builder.Append(_computerNameProvider.GetComputerName());
		}

		private static IComputerNameProvider _computerNameProvider = new DefaultComputerNameProvider();
	}
}