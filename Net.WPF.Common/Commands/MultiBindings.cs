using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using MultiBinding = Wpf.UI.Commands.MultiBinding;

namespace Wpf.UI.Commands
{
	/// <summary>
	/// 	Manages the construction of multiple MultiBinding instances
	/// </summary>
	[ContentProperty("Bindings")]
	public class MultiBindings : FrameworkElement
	{
		// ReSharper disable ConditionIsAlwaysTrueOrFalse
		// ReSharper disable HeuristicUnreachableCode

#if !SILVERLIGHT
		private FrameworkElement _targetElement;
#endif

		// ReSharper restore MemberCanBePrivate.Global
		public MultiBindings()
		{
			Bindings = new ObservableCollection<MultiBinding>();
		}

#if !SILVERLIGHT
	private void Loaded(object sender, RoutedEventArgs e)
	{
	  _targetElement.Loaded -= Loaded;
	  foreach (MultiBinding binding in Bindings)
	  {
		FieldInfo field = _targetElement.GetType().GetField(binding.TargetProperty + "Property", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
		if (field == null) continue;

		var newBinding = new System.Windows.Data.MultiBinding
														  {
															Converter = binding.Converter,
															ConverterParameter = binding.ConverterParameter
														  };
		foreach (BindingBase bindingBase in binding.Bindings)
		{
		  newBinding.Bindings.Add(bindingBase);
		}

		DependencyProperty dp = (DependencyProperty)field.GetValue(_targetElement);

		BindingOperations.SetBinding(_targetElement, dp, newBinding);
	  }
	}
#endif

		/// <summary>
		/// 	Gets / sets the collection of MultiBindings
		/// </summary>
		public ObservableCollection<MultiBinding> Bindings { get; set; }

		/// <summary>
		/// 	Sets the DataContext of each of the MultiBinding instances
		/// </summary>
		public void SetDataContext(object dataContext)
		{
			foreach (var multiBinding in Bindings)
			{
				multiBinding.DataContext = dataContext;
			}
		}

		/// <summary>
		/// 	Initialises each of the MultiBindings, and binds their ConvertedValue to the given target property.
		/// </summary>
		public void Initialize(FrameworkElement targetElement)
		{
#if !SILVERLIGHT
			_targetElement = targetElement;
			_targetElement.Loaded += Loaded;
#else
			const BindingFlags DpFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

			foreach (var multiBinding in Bindings)
			{
				multiBinding.Initialise(targetElement);

				// find the target dependency property
				Type targetType;
				string targetProperty;

				// assume it is an attached property if the dot syntax is used.
				if (IsAttachedProperty(multiBinding))
				{
					string aggregate;
					var parts = ParseType(multiBinding, out aggregate);

					targetType = FindTargetType(aggregate) ?? Assembly
						.GetExecutingAssembly()
						.GetTypes()
						.SingleOrDefault(x => x.Name == aggregate);
					if (targetType == null)
					{
						var message = string.Format("Type {0} was not found in the executing  assembly", aggregate);
						throw new TypeLoadException(message);
					}

					targetProperty = parts[1];
				}
				else
				{
					targetType = targetElement.GetType();
					targetProperty = multiBinding.TargetProperty;
				}

				var sourceFields = targetType.GetFields(DpFlags);
				var targetDependencyPropertyField = sourceFields.First(i => i.Name == targetProperty + "Property");
				var targetDependencyProperty = targetDependencyPropertyField.GetValue(null) as DependencyProperty;

				if (targetDependencyProperty == null)
				{
					throw new NullReferenceException("targetDependencyProperty");
				}

				// bind the ConvertedValue of our MultiBinding instance to the target property of our targetElement
				var binding = new Binding("ConvertedValue")
				{
					Source = multiBinding,
					Mode = multiBinding.Mode
				};
				targetElement.SetBinding(targetDependencyProperty, binding);
			}
#endif
		}

		private static Type FindTargetType(string aggregate)
		{
			return Type.GetType("System.Windows.Controls." +
				aggregate +
					", System.Windows, Version=2.0.5.0, Culture=neutral, " +
						"PublicKeyToken=7cec85d7bea7798e");
		}

		private static string[] ParseType(MultiBinding multiBinding, out string aggregate)
		{
			// split to find the type and property name

			var parts = multiBinding.TargetProperty.Split('.');
			aggregate = parts.Take(parts.Length - 1).Aggregate("", (x, y) => string.Concat(x, ".", y));
			if (aggregate.StartsWith("."))
			{
				aggregate = aggregate.Substring(1);
			}
			return parts;
		}

		private static bool IsAttachedProperty(MultiBinding multiBinding)
		{
			return multiBinding.TargetProperty.Contains(".");
		}
	}
}