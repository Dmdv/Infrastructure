using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Extensions;

namespace Wpf.UI.Commands
{
	/// <summary>
	/// TabItemClick.
	/// </summary>
	public static class TabItemClick
	{
		// ReSharper disable MemberCanBePrivate.Global

		/// <summary>
		/// 	ItemClickedBehavior.
		/// </summary>
		public static readonly DependencyProperty TabSelectionCommandBehaviorProperty =
			DependencyProperty.RegisterAttached("TabSelectionCommandBehaviorProperty",
				typeof (TabSelectionCommandBehavior),
				typeof (TabItemClick),
				null);

		/// <summary>
		/// 	Command on click.
		/// </summary>
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.RegisterAttached("Command",
				typeof (ICommand),
				typeof (TabItemClick),
				new PropertyMetadata(CommandPropertyChanged));

		/// <summary>
		/// 	GetCommand.
		/// </summary>
		/// <param name="obj"> </param>
		/// <returns> </returns>
		public static ICommand GetCommand(DependencyObject obj)
		{
			return obj.GetValue(CommandProperty).Cast<ICommand>();
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
			var targetObject = dependencyObject.Cast<TabControl>();

			if (targetObject != null)
			{
				GetOrCreateBehavior(targetObject).Command = e.NewValue.Cast<ICommand>();
			}
		}

		private static TabSelectionCommandBehavior GetOrCreateBehavior(TabControl targetObject)
		{
			var behavior = targetObject.GetValue(TabSelectionCommandBehaviorProperty) as TabSelectionCommandBehavior;

			if (behavior == null)
			{
				behavior = new TabSelectionCommandBehavior(targetObject);
				targetObject.SetValue(TabSelectionCommandBehaviorProperty, behavior);
			}

			return behavior;
		}
	}
}