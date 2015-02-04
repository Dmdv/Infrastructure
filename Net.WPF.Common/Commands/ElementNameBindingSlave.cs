using System.Windows;
using System.Windows.Data;

namespace Wpf.UI.Commands
{
	/// <summary>
	/// 	ElementNameBindingSlave.
	/// </summary>
	public class ElementNameBindingSlave : BindingSlave
	{
		private readonly Binding _binding;
		private readonly FrameworkElement _multiBindingTarget;

		/// <summary>
		/// 	The source element named in the ElementName binding
		/// </summary>
		private FrameworkElement _elementNameSource;

		public ElementNameBindingSlave(FrameworkElement target, Binding binding)
		{
			_multiBindingTarget = target;
			_binding = binding;

			// try to locate the named element
			ResolveElementNameBinding();

			_multiBindingTarget.LayoutUpdated += (sender, e) => ResolveElementNameBinding();
		}

		/// <summary>
		/// 	Try to locate the named element. If the element can be located, create the required binding.
		/// </summary>
		private void ResolveElementNameBinding()
		{
			_elementNameSource = _multiBindingTarget.FindName(_binding.ElementName) as FrameworkElement;
			if (_elementNameSource != null)
			{
				SetBinding(ValueProperty, new Binding
				{
					Source = _elementNameSource,
					Path = _binding.Path,
					Converter = _binding.Converter,
					ConverterParameter = _binding.ConverterParameter
				});
			}
		}
	}
}