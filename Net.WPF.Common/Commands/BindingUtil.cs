using System.Windows;
using System.Windows.Data;

namespace Wpf.UI.Commands
{
	/// <summary>
	/// 	BindingUtil.
	/// </summary>
	public class BindingUtil
	{
		// ReSharper disable MemberCanBePrivate.Global

		/// <summary>
		/// DataContextPiggyBack Attached Dependency Property, 
		/// used as a mechanism for exposing DataContext changed events
		/// </summary>
		public static readonly DependencyProperty DataContextPiggyBackProperty =
			DependencyProperty.RegisterAttached("DataContextPiggyBack", typeof (object), typeof (BindingUtil),
			new PropertyMetadata(null, OnDataContextPiggyBackChanged));

		public static readonly DependencyProperty MultiBindingsProperty =
			DependencyProperty.RegisterAttached("MultiBindings",
			typeof (MultiBindings), typeof (BindingUtil), new PropertyMetadata(null, OnMultiBindingsChanged));

		/// <summary>
		/// 	GetDataContextPiggyBack.
		/// </summary>
		/// <param name="d"> </param>
		/// <returns> </returns>
		public static object GetDataContextPiggyBack(DependencyObject d)
		{
			return d.GetValue(DataContextPiggyBackProperty);
		}

		/// <summary>
		/// 	SetDataContextPiggyBack.
		/// </summary>
		/// <param name="d"> </param>
		/// <param name="value"> </param>
		public static void SetDataContextPiggyBack(DependencyObject d, object value)
		{
			d.SetValue(DataContextPiggyBackProperty, value);
		}

		/// <summary>
		/// 	MultiBindings/.
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public static MultiBindings GetMultiBindings(DependencyObject obj)
		{
			return (MultiBindings) obj.GetValue(MultiBindingsProperty);
		}

		/// <summary>
		/// 	MultiBindings/.
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="value"> </param>
		public static void SetMultiBindings(DependencyObject obj, MultiBindings value)
		{
			obj.SetValue(MultiBindingsProperty, value);
		}

		/// <summary>
		/// 	Handles changes to the DataContextPiggyBack property.
		/// </summary>
		private static void OnDataContextPiggyBackChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var targetElement = d as FrameworkElement;

			// whenever the targeElement DataContext is changed, copy the updated property
			// value to our MultiBinding.
			var relay = GetMultiBindings(targetElement);
			relay.SetDataContext(targetElement.DataContext);
		}

		/// <summary>
		/// 	Invoked when the MultiBinding property is set on a framework element
		/// </summary>
		private static void OnMultiBindingsChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
		{
			var targetElement = depObj as FrameworkElement;

			// bind the target elements DataContext, to our DataContextPiggyBack property
			// this allows us to get property changed events when the targetElement
			// DataContext changes
			targetElement.SetBinding(DataContextPiggyBackProperty, new Binding());

			var bindings = GetMultiBindings(targetElement);

			bindings.Initialize(targetElement);
		}

		// ReSharper restore MemberCanBePrivate.Global
	}
}