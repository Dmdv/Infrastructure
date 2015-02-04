using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Wpf.UI.Commands
{
	/// <summary>
	/// Implements MultiBinding by creating a BindingSlave instance for each of the Bindings. 
	/// PropertyChanged events for the BindingSlae.Value property are handled, and the 
	/// IMultiValueConveter is used to compute the converted value.
	/// </summary>
	[ContentProperty("Bindings")]
	public sealed class MultiBinding : Panel, INotifyPropertyChanged
	{
		public static readonly DependencyProperty ConvertedValueProperty =
			DependencyProperty.Register("ConvertedValue", typeof (object), typeof (MultiBinding),
			new PropertyMetadata(null, OnConvertedValuePropertyChanged));

		/// <summary>
		/// Indicates whether the converted value property is currently being 
		/// updated as a result of one of the BindingSlave.Value properties changing
		/// </summary>
		private bool _updatingConvertedValue;

		public MultiBinding()
		{
			Bindings = new BindingCollection();
		}

		/// <summary>
		/// 	This dependency property is set to the resulting output of the associated Converter.
		/// </summary>
		public object ConvertedValue
		{
			get { return GetValue(ConvertedValueProperty); }
			set { SetValue(ConvertedValueProperty, value); }
		}

		/// <summary>
		/// 	The BindingMode
		/// </summary>
		public BindingMode Mode { get; set; }

		/// <summary>
		/// 	The target property on the element which this MultiBinding is assocaited with.
		/// </summary>
		public string TargetProperty { get; set; }

		/// <summary>
		/// 	The Converter which is invoked to compute the result of the multiple bindings
		/// </summary>
		public IMultiValueConverter Converter { get; set; }

		/// <summary>
		/// 	The configuration parameter supplied to the converter
		/// </summary>
		public object ConverterParameter { get; set; }

		/// <summary>
		/// 	The bindings, the result of which are supplied to the converter.
		/// </summary>
		public BindingCollection Bindings { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// 	Creates a BindingSlave for each Binding and binds the Value accordingly.
		/// </summary>
		public void Initialise(FrameworkElement targetElement)
		{
			Children.Clear();
			foreach (Binding binding in Bindings)
			{
				BindingSlave slave;

				// create a binding slave instance 
				if (!string.IsNullOrEmpty(binding.ElementName))
				{
					// create an element name binding slave, this slave will resolve the 
					// binding source reference and construct a suitable binding.
					slave = new ElementNameBindingSlave(targetElement, binding);
				}
				else
				{
					slave = new BindingSlave();
					slave.SetBinding(BindingSlave.ValueProperty, binding);
				}
				slave.PropertyChanged += SlavePropertyChanged;
				Children.Add(slave);
			}
		}

		/// <summary>
		/// 	Handles propety changes for the ConvertedValue property
		/// </summary>
		private void OnConvertedValuePropertyChanged()
		{
			OnPropertyChanged("ConvertedValue");

			// if the value is being updated, but not due to one of the multibindings
			// then the target property has changed.
			if (!_updatingConvertedValue)
			{
				// convert back
				var convertedValues = Converter.ConvertBack(
					ConvertedValue, 
					null, 
					ConverterParameter, 
					CultureInfo.InvariantCulture);

				// update all the binding slaves
				if (Children.Count == convertedValues.Length)
				{
					for (var index = 0; index < convertedValues.Length; index++)
					{
						((BindingSlave) Children[index]).Value = convertedValues[index];
					}
				}
			}
		}

		private static void OnConvertedValuePropertyChanged(DependencyObject depObj, 
			DependencyPropertyChangedEventArgs e)
		{
			var relay = depObj as MultiBinding;
			if (relay != null)
			{
				relay.OnConvertedValuePropertyChanged();
			}
		}

		/// <summary>
		/// 	Invoked when any of the BindingSlave's Value property changes.
		/// </summary>
		private void SlavePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			UpdateConvertedValue();
		}

		/// <summary>
		/// 	Uses the Converter to update the ConvertedValue in order to reflect the current state of the bindings.
		/// </summary>
		private void UpdateConvertedValue()
		{
			var values = Children.OfType<BindingSlave>().Select(x => x.Value).ToArray();
			_updatingConvertedValue = true;
			ConvertedValue = Converter.Convert(values, typeof (object), ConverterParameter, CultureInfo.CurrentCulture);
			_updatingConvertedValue = false;
		}

		private void OnPropertyChanged(string name)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}