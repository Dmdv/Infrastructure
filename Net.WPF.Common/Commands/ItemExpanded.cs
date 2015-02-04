using System.Windows;
using System.Windows.Input;
using Wpf.UI.Controls;

namespace Wpf.UI.Commands
{
	/// <summary>
	/// 	TreeView ItemExpanded поведение.
	/// </summary>
	public static class ItemExpanded
	{
		// ReSharper disable MemberCanBePrivate.Global

		public static readonly DependencyProperty ItemExpandedBehaviorProperty =
			DependencyProperty.RegisterAttached("ItemExpandedBehaviorProperty",
				typeof (ItemExpandedCommandBehavior),
				typeof (ItemExpandedCommandBehavior),
				null);

		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.RegisterAttached("Command",
				typeof (ICommand),
				typeof (ItemExpanded),
				new PropertyMetadata(CommandPropertyChanged));

		/// <summary>
		/// 	GetCommand.
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public static ICommand GetCommand(DependencyObject obj)
		{
			return (ICommand) obj.GetValue(CommandProperty);
		}

		/// <summary>
		/// 	SetCommand.
		/// </summary>
		/// <param name="obj"> </param>
		/// <param name="value"> </param>
		public static void SetCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(CommandProperty, value);
		}

		private static void CommandPropertyChanged(
			DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs e)
		{
			var targetObject = dependencyObject as LazyTreeView;

			if (targetObject != null)
			{
				GetOrCreateBehavior(targetObject).Command = e.NewValue as ICommand;
			}
		}

		private static ItemExpandedCommandBehavior GetOrCreateBehavior(LazyTreeView targetObject)
		{
			var behavior = targetObject.GetValue(ItemExpandedBehaviorProperty) as ItemExpandedCommandBehavior;

			if (behavior == null)
			{
				behavior = new ItemExpandedCommandBehavior(targetObject);
				targetObject.SetValue(ItemExpandedBehaviorProperty, behavior);
			}

			return behavior;
		}
	}
}