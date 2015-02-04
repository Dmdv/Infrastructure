using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Wpf.UI.Commands
{
	/// <summary>
	/// Static Class that holds all Dependency Properties and Static methods to allow the 
	/// Click event of the ButtonBase class to be attached to a Command.
	/// </summary>
	/// <remarks>
	/// This class is required, because Silverlight doesn't have native support for Commands.
	/// </remarks>
	public static class Click
	{
		// ReSharper disable MemberCanBePrivate.Global
		// ReSharper disable MemberCanBeProtected.Global
		// ReSharper disable VirtualMemberNeverOverriden.Global
		// ReSharper disable UnusedMember.Global

		private static readonly DependencyProperty _clickCommandBehaviorProperty = DependencyProperty.RegisterAttached(
			"ClickCommandBehavior",
			typeof (ButtonBaseClickCommandBehavior),
			typeof (Click),
			null);

		/// <summary>
		/// 	Command to execute on click event.
		/// </summary>
		public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
			"Command",
			typeof (ICommand),
			typeof (Click),
			new PropertyMetadata(OnSetCommandCallback));

		/// <summary>
		/// 	Command parameter to supply on command execution.
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
			"CommandParameter",
			typeof (object),
			typeof (Click),
			new PropertyMetadata(OnSetCommandParameterCallback));

		/// <summary>
		/// 	Identifies the IsConfirm attached property.
		/// </summary>
		public static readonly DependencyProperty IsConfirmProperty = DependencyProperty.RegisterAttached(
			"IsConfirm",
			typeof (bool),
			typeof (Click),
			new PropertyMetadata(OnSetIsConfirmCallback));

		/// <summary>
		/// 	Identifies the ConfirmCaption attached property.
		/// </summary>
		public static readonly DependencyProperty ConfirmCaptionProperty = DependencyProperty.RegisterAttached(
			"ConfirmCaption",
			typeof (string),
			typeof (Click),
			new PropertyMetadata(OnSetConfirmCaptionCallback));

		/// <summary>
		/// 	Identifies the ConfirmMessage attached property.
		/// </summary>
		public static readonly DependencyProperty ConfirmMessageProperty = DependencyProperty.RegisterAttached(
			"ConfirmMessage",
			typeof (string),
			typeof (Click),
			new PropertyMetadata(OnSetConfirmMessageCallback));

		/// <summary>
		/// 	Gets the value of the ConfirmMessage attached property for a given dependency object.
		/// </summary>
		/// <param name="obj"> The object for which the property value is read. </param>
		/// <returns> The value of the ConfirmMessage property of the specified object. </returns>
		public static string GetConfirmMessage(DependencyObject obj)
		{
			return (string) obj.GetValue(ConfirmMessageProperty);
		}

		/// <summary>
		/// 	Sets the value of the ConfirmMessage attached property for a given dependency object.
		/// </summary>
		/// <param name="obj"> The object to which the property value is written. </param>
		/// <param name="value"> Sets the ConfirmMessage value of the specified object. </param>
		public static void SetConfirmMessage(DependencyObject obj, string value)
		{
			obj.SetValue(ConfirmMessageProperty, value);
		}

		/// <summary>
		/// 	Gets the value of the ConfirmCaption attached property for a given dependency object.
		/// </summary>
		/// <param name="obj"> The object for which the property value is read. </param>
		/// <returns> The value of the ConfirmCaption property of the specified object. </returns>
		public static string GetConfirmCaption(DependencyObject obj)
		{
			return (string) obj.GetValue(ConfirmCaptionProperty);
		}

		/// <summary>
		/// 	Sets the value of the ConfirmCaption attached property for a given dependency object.
		/// </summary>
		/// <param name="obj"> The object to which the property value is written. </param>
		/// <param name="value"> Sets the ConfirmCaption value of the specified object. </param>
		public static void SetConfirmCaption(DependencyObject obj, string value)
		{
			obj.SetValue(ConfirmCaptionProperty, value);
		}

		/// <summary>
		/// 	Gets the value of the IsConfirm attached property for a given dependency object.
		/// </summary>
		/// <param name="obj"> The object for which the property value is read. </param>
		/// <returns> The value of the IsConfirm property of the specified object. </returns>
		public static bool GetIsConfirm(DependencyObject obj)
		{
			return (bool) obj.GetValue(IsConfirmProperty);
		}

		/// <summary>
		/// 	Sets the value of the IsConfirm attached property for a given dependency object.
		/// </summary>
		/// <param name="obj"> The object to which the property value is written. </param>
		/// <param name="value"> Sets the IsConfirm value of the specified object. </param>
		public static void SetIsConfirm(DependencyObject obj, bool value)
		{
			obj.SetValue(IsConfirmProperty, value);
		}

		/// <summary>
		/// 	Sets the <see cref="ICommand" /> to execute on the click event.
		/// </summary>
		/// <param name="buttonBase"> ButtonBase dependency object to attach command </param>
		/// <param name="command"> Command to attach </param>
		public static void SetCommand(ButtonBase buttonBase, ICommand command)
		{
			buttonBase.SetValue(CommandProperty, command);
		}

		/// <summary>
		/// 	Retrieves the <see cref="ICommand" /> attached to the <see cref="ButtonBase" /> .
		/// </summary>
		/// <param name="buttonBase"> ButtonBase containing the Command dependency property </param>
		/// <returns> The value of the command attached </returns>
		public static ICommand GetCommand(ButtonBase buttonBase)
		{
			return buttonBase.GetValue(CommandProperty) as ICommand;
		}

		/// <summary>
		/// 	Sets the value for the CommandParameter attached property on the provided <see cref="ButtonBase" /> .
		/// </summary>
		/// <param name="buttonBase"> ButtonBase to attach CommandParameter </param>
		/// <param name="parameter"> Parameter value to attach </param>
		public static void SetCommandParameter(ButtonBase buttonBase, object parameter)
		{
			buttonBase.SetValue(CommandParameterProperty, parameter);
		}

		/// <summary>
		/// 	Gets the value in CommandParameter attached property on the provided <see cref="ButtonBase" />
		/// </summary>
		/// <param name="buttonBase"> ButtonBase that has the CommandParameter </param>
		/// <returns> The value of the property </returns>
		public static object GetCommandParameter(ButtonBase buttonBase)
		{
			return buttonBase.GetValue(CommandParameterProperty);
		}

		private static void OnSetConfirmMessageCallback(
			DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var buttonBase = dependencyObject as ButtonBase;
			if (buttonBase != null)
			{
				var behavior = GetOrCreateBehavior(buttonBase);
				behavior.ConfirmMessage = e.NewValue as string;
			}
		}

		private static void OnSetConfirmCaptionCallback(
			DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var buttonBase = dependencyObject as ButtonBase;
			if (buttonBase != null)
			{
				var behavior = GetOrCreateBehavior(buttonBase);
				behavior.ConfirmCaption = e.NewValue as string;
			}
		}

		private static void OnSetCommandCallback(
			DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs e)
		{
			var buttonBase = dependencyObject as ButtonBase;
			if (buttonBase != null)
			{
				var behavior = GetOrCreateBehavior(buttonBase);
				behavior.Command = e.NewValue as ICommand;
			}
		}

		private static void OnSetCommandParameterCallback(
			DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs e)
		{
			var buttonBase = dependencyObject as ButtonBase;
			if (buttonBase != null)
			{
				var behavior = GetOrCreateBehavior(buttonBase);
				behavior.CommandParameter = e.NewValue;
			}
		}

		private static void OnSetIsConfirmCallback(DependencyObject dependencyObject, 
			DependencyPropertyChangedEventArgs e)
		{
			var buttonBase = dependencyObject as ButtonBase;
			if (buttonBase != null)
			{
				var behavior = GetOrCreateBehavior(buttonBase);
				behavior.IsConfirm = (bool) e.NewValue;
			}
		}

		private static ButtonBaseClickCommandBehavior GetOrCreateBehavior(ButtonBase buttonBase)
		{
			var behavior = buttonBase.GetValue(_clickCommandBehaviorProperty) as ButtonBaseClickCommandBehavior;
			if (behavior == null)
			{
				behavior = new ButtonBaseClickCommandBehavior(buttonBase);
				buttonBase.SetValue(_clickCommandBehaviorProperty, behavior);
			}

			return behavior;
		}
	}
}