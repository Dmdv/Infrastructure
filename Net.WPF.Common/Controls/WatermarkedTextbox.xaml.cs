using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.UI.Controls
{
	/// <summary>
	/// A user control to provide a filter for a list view
	/// </summary>
	public partial class WatermarkedTextbox
	{
		// ReSharper disable MemberCanBePrivate.Global
		// ReSharper disable UnusedMember.Global

		/// <summary>
		/// The Text displayed in the TextBox control
		/// </summary>
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register(
				"Text",
				typeof (string),
				typeof (WatermarkedTextbox),
				new PropertyMetadata(TextChangedHandler));

		/// <summary>
		/// The Watermark displayed in the TextBox control
		/// </summary>
		public static readonly DependencyProperty WatermarkProperty =
			DependencyProperty.Register(
				"Watermark",
				typeof (string),
				typeof (WatermarkedTextbox),
				new PropertyMetadata("Enter text here", WatermarkChangedHandler));

		/// <summary>
		/// A command to fire when the text changes
		/// </summary>
		public static readonly DependencyProperty TextChangedCommandProperty =
			DependencyProperty.Register(
				"TextChangedCommand",
				typeof (ICommand),
				typeof (WatermarkedTextbox),
				null);

		/// <summary>
		/// A parameter to pass to the TextChangedCommand
		/// </summary>
		public static readonly DependencyProperty TextChangedCommandParameterProperty =
			DependencyProperty.Register(
				"TextChangedCommandParameter",
				typeof (object),
				typeof (WatermarkedTextbox),
				null);

		/// <summary>
		/// A style to assign to the TextBox in this control
		/// </summary>
		public static readonly DependencyProperty TextBoxStyleProperty =
			DependencyProperty.Register(
				"TextBoxStyle",
				typeof (Style),
				typeof (WatermarkedTextbox),
				new PropertyMetadata(TextBoxStyleChanged));

		/// <summary>
		/// Initializes a new instance of the <see cref="WatermarkedTextbox"/> class.
		/// </summary>
		public WatermarkedTextbox()
		{
			// Required to initialize variables
			InitializeComponent();
		}

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text bound to our textbox.</value>
		public string Text
		{
			get { return (string) GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Gets or sets the text box style.
		/// </summary>
		/// <value>The text box style.</value>
		public Style TextBoxStyle
		{
			get { return (Style) GetValue(TextBoxStyleProperty); }
			set { SetValue(TextBoxStyleProperty, value); }
		}

		/// <summary>
		/// Gets or sets the text changed command.
		/// </summary>
		/// <value>The text changed command.</value>
		public ICommand TextChangedCommand
		{
			get { return (ICommand) GetValue(TextChangedCommandProperty); }
			set { SetValue(TextChangedCommandProperty, value); }
		}

		/// <summary>
		/// Gets or sets the text changed command parameter.
		/// </summary>
		/// <value>The text changed command parameter.</value>
		public object TextChangedCommandParameter
		{
			get { return GetValue(TextChangedCommandParameterProperty); }
			set { SetValue(TextChangedCommandParameterProperty, value); }
		}

		/// <summary>
		/// Gets or sets the Watermark.
		/// </summary>
		/// <value>The Watermark.</value>
		public string Watermark
		{
			get { return (string) GetValue(WatermarkProperty); }
			set { SetValue(WatermarkProperty, value); }
		}

		private static void TextBoxStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as WatermarkedTextbox;
			if (control == null)
			{
				return;
			}

			control.TextEntry.Style = (Style) e.NewValue;
		}

		private static void TextChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as WatermarkedTextbox;
			if (control == null)
			{
				return;
			}

			var value = (null == e.NewValue) ? string.Empty : (string) e.NewValue;

			var text = control.TextEntry.Text;
			if (text != value)
			{
				control.TextEntry.Text = value;
			}

			if (!string.IsNullOrEmpty(value))
			{
				VisualStateManager.GoToState(control, "WatermarkHidden", true);
				VisualStateManager.GoToState(control, "ButtonVisible", true);
			}
		}

		private static void WatermarkChangedHandler(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var control = dependencyObject as WatermarkedTextbox;
			if (control == null)
			{
				return;
			}

			var value = (null == e.NewValue) ? string.Empty : (string) e.NewValue;

			var text = control.WatermarkText.Text;
			if (text != value)
			{
				control.WatermarkText.Text = value;
			}
		}

		private void ClearFilterButtonClick(object sender, RoutedEventArgs e)
		{
			TextEntry.Text = string.Empty;
		}

		private void TextEntryGotFocus(object sender, RoutedEventArgs e)
		{
			VisualStateManager.GoToState(this, "WatermarkHidden", false);
			_isFocused = true;
		}

		private void TextEntryLostFocus(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(TextEntry.Text))
			{
				VisualStateManager.GoToState(this, "WatermarkVisible", true);
			}

			_isFocused = false;
		}

		private void TextEntryTextChanged(object sender, TextChangedEventArgs e)
		{
			// Update our Text dependency property
			Text = TextEntry.Text;

			// Check if we need to update our states
			if (string.IsNullOrEmpty(TextEntry.Text))
			{
				VisualStateManager.GoToState(this, "ButtonHidden", true);
				if (!_isFocused)
				{
					VisualStateManager.GoToState(this, "WatermarkVisible", true);
				}
			}
			else
			{
				VisualStateManager.GoToState(this, "ButtonVisible", true);
				VisualStateManager.GoToState(this, "WatermarkHidden", true);
			}

			var textChangedCommand = TextChangedCommand;
			if (textChangedCommand != null)
			{
				textChangedCommand.Execute(TextChangedCommandParameter);
			}
		}

		private bool _isFocused;
	}
}