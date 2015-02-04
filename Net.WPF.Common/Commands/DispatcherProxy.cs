using System;
using System.Windows;
using System.Windows.Threading;

namespace EditorClient.Commands
{
	/// <summary>
	/// Hides the dispatcher mis-match between Silverlight and .Net, largely so code reads a bit easier
	/// </summary>
	internal class DispatcherProxy
	{
		private readonly Dispatcher _innerDispatcher;

		private DispatcherProxy(Dispatcher dispatcher)
		{
			_innerDispatcher = dispatcher;
		}

		/// <summary>
		/// CreateDispatcher.
		/// </summary>
		public static DispatcherProxy CreateDispatcher()
		{
			return new DispatcherProxy(Application.Current.Dispatcher);
		}

		/// <summary>
		/// CheckAccess.
		/// </summary>
		public bool CheckAccess()
		{
			return _innerDispatcher.CheckAccess();
		}

		/// <summary>
		/// BeginInvoke.
		/// </summary>
		public DispatcherOperation BeginInvoke(Delegate method, params object[] args)
		{
			return _innerDispatcher.BeginInvoke(method, args);
		}
	}
}