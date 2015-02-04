using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using EditorClient.ServiceReference;

namespace EditorClient.ViewModels
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		private static bool? _isInDesignMode;

		protected BaseViewModel()
		{
			Logger = new DefaultLogger();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected bool IsInDesignMode
		{
			get { return IsInDesignModeStatic; }
		}

		protected Dispatcher Dispatcher
		{
			get { return Application.Current.Dispatcher; }
		}

		protected ILogger Logger { get; set; }

		protected void OnPropertyChanged(string property)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(property));
		}

		/// <summary>
		/// Gets a value indicating whether the control is in design mode (running in Blend or Visual Studio).
		/// </summary>
		private static bool IsInDesignModeStatic
		{
			get
			{
				if (!_isInDesignMode.HasValue)
				{
#if SILVERLIGHT
					_isInDesignMode = DesignerProperties.IsInDesignTool;
#else
					var prop = DesignerProperties.IsInDesignModeProperty;
					_isInDesignMode = (bool) DependencyPropertyDescriptor
						                         .FromProperty(prop, typeof (FrameworkElement))
						                         .Metadata.DefaultValue;
#endif
				}
				return _isInDesignMode.Value;
			}
		}

		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, e);
		}
	}
}