using System.ComponentModel;
using System.Windows;

namespace Wpf.UI.Commands
{
	/// <summary>
	/// 	Dependency business object.
	/// </summary>
	/// <typeparam name="T"> </typeparam>
	public class BusinessDependencyObject<T> : DependencyObject, INotifyPropertyChanged
	{
		/// <summary>
		/// 	Identifies the Value property of the ObservableObject
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof (T), typeof (BusinessDependencyObject<T>),
				new PropertyMetadata(ValueChangedCallback));

		/// <summary>
		/// 	The value that's wrapped inside the ObservableObject.
		/// </summary>
		public T Value
		{
			get { return (T) GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// 	Event that gets invoked when the Value property changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var thisInstance = (BusinessDependencyObject<T>) d;
			var eventHandler = thisInstance.PropertyChanged;
			if (eventHandler != null)
			{
				eventHandler(thisInstance, new PropertyChangedEventArgs("Value"));
			}
		}
	}
}