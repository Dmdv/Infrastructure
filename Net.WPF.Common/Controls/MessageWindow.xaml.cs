using System;
using System.Windows;
using Common.Extensions;

namespace Wpf.UI.Controls
{
	/// <summary>
	/// MessageWindow.
	/// </summary>
	public partial class MessageWindow
	{
		/// <summary>
		/// The <see cref="Message" /> dependency property's name.
		/// </summary>
		private const string MessagePropertyName = "Message";

		/// <summary>
		/// The <see cref="AcceptText" /> dependency property's name.
		/// </summary>
		private const string AcceptTextPropertyName = "AcceptText";

		/// <summary>
		/// The <see cref="CancelText" /> dependency property's name.
		/// </summary>
		private const string CancelTextPropertyName = "CancelText";

		/// <summary>
		/// Identifies the <see cref="CancelText" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty CancelTextProperty =
			DependencyProperty.Register(
				CancelTextPropertyName,
				typeof (string),
				typeof (MessageWindow),
				new PropertyMetadata("Cancel", OnCancelTextCallbackChanged));

		/// <summary>
		/// Identifies the <see cref="AcceptText" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty OkTextProperty =
			DependencyProperty.Register(
				AcceptTextPropertyName,
				typeof (string),
				typeof (MessageWindow),
				new PropertyMetadata("Ok", OnOkTextCallbackChanged));

		/// <summary>
		/// Identifies the <see cref="Message" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty MessageProperty =
			DependencyProperty.Register(
				MessagePropertyName,
				typeof (string),
				typeof (MessageWindow),
				new PropertyMetadata(OnMessageChanged));

		/// <summary>
		/// Closed event.
		/// </summary>
		public new event EventHandler<DialogArgs> Closed;

		public MessageWindow()
		{
			InitializeComponent();
			Title = "Dialog";
			Message = "Message";
		}

		/// <summary>
		/// Gets or sets the value of the <see cref="AcceptText" />
		/// property. This is a dependency property.
		/// </summary>
		public string AcceptText
		{
			get { return (string) GetValue(OkTextProperty); }
			set { SetValue(OkTextProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of the <see cref="CancelText" />
		/// property. This is a dependency property.
		/// </summary>
		public string CancelText
		{
			get { return (string) GetValue(CancelTextProperty); }
			set { SetValue(CancelTextProperty, value); }
		}

		/// <summary>
		/// Gets or sets the value of the <see cref="Message" />
		/// property. This is a dependency property.
		/// </summary>
		public string Message
		{
			get { return (string) GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}

		public string Title { get; set; }

		/// <summary>
		/// Shows info message with OK Button.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="caption"></param>
		public static void Show(string message, string caption)
		{
			var win = new MessageWindow
			{
				Title = caption,
				Message = message,
				CancelButton =
				{
					Visibility = Visibility.Collapsed
				}
			};
			win.Show();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			InvokeClosed(new DialogArgs
			{
				Result = DialogResult
			});
		}

		private static void OnCancelTextCallbackChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.Cast<MessageWindow>().CancelButton.Content = e.NewValue;
		}

		// public void ShowModalDialog()
		// {
		//    var waitHandle = new AutoResetEvent(false);
		//    var cw = new MessageWindow {Message = Message, Title = Title};

		////    DispatcherHelper.UIDispatcher.BeginInvoke(() =>
		//                               {
		//                                   cw.Closed += (s, e) => waitHandle.Set();
		//                                   cw.Show();
		//                               });
		//    waitHandle.WaitOne();

		////    DialogResult = cw.DialogResult;
		// }

		private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var messageWindow = d.Cast<MessageWindow>();
			messageWindow.TextContent.Text = e.NewValue.Cast<string>();
		}

		private static void OnOkTextCallbackChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.Cast<MessageWindow>().OKButton.Content = e.NewValue;
		}

		private void CancelButtonClick(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void InvokeClosed(DialogArgs e)
		{
			var handler = Closed;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		private void OkButtonClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}