using System.Windows;
using System.Windows.Input;
using Wpf.UI.Controls;

namespace Wpf.UI.Commands
{
	/// <summary>
	/// 	Расширение поведения TreeView.
	/// </summary>
	public static class ItemClicked
	{
		// ReSharper disable MemberCanBePrivate.Global

		/// <summary>
		/// 	ItemClickedBehavior.
		/// </summary>
		public static readonly DependencyProperty ItemClickedBehaviorProperty =
			DependencyProperty.RegisterAttached("ItemClickedBehaviorProperty",
			typeof (ItemClickedCommandBehavior), typeof (ItemClicked), null);

		/// <summary>
		/// 	Command on click.
		/// </summary>
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.RegisterAttached("Command", typeof (ICommand),
			typeof (ItemClicked), new PropertyMetadata(CommandPropertyChanged));

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

		private static ItemClickedCommandBehavior GetOrCreateBehavior(LazyTreeView targetObject)
		{
			var behavior = targetObject.GetValue(ItemClickedBehaviorProperty) as ItemClickedCommandBehavior;

			if (behavior == null)
			{
				behavior = new ItemClickedCommandBehavior(targetObject);
				targetObject.SetValue(ItemClickedBehaviorProperty, behavior);
			}

			return behavior;
		}
	}
}