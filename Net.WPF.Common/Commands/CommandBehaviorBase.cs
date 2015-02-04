using System;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.UI.Controls;

namespace Wpf.UI.Commands
{
	/// <summary>
	/// 	Base behavior to handle connecting a <see cref="Control" /> to a Command.
	/// </summary>
	/// <typeparam name="T"> The target object must derive from Control </typeparam>
	/// <remarks>
	/// CommandBehaviorBase can be used to provide new behaviors similar to 
	/// <see cref="ButtonBaseClickCommandBehavior" /> .
	/// </remarks>
	public class CommandBehaviorBase<T> where T : Control
	{
		private readonly EventHandler _commandCanExecuteChangedHandler;
		private readonly MessageWindow _messageWindow = new MessageWindow();
		private readonly WeakReference _targetObject;
		private ICommand _command;
		private object _commandParameter;

		/// <summary>
		/// 	Constructor specifying the target object.
		/// </summary>
		/// <param name="targetObject"> The target object the behavior is attached to. </param>
		public CommandBehaviorBase(T targetObject)
		{
			_messageWindow.Closed += MessageWindowClosed;

			IsConfirm = false;
			ConfirmMessage = "Are you sure you want to apply changes?";
			ConfirmCaption = "Confirm operation";

			_targetObject = new WeakReference(targetObject);
			_commandCanExecuteChangedHandler = CommandCanExecuteChanged;
		}

		/// <summary>
		/// 	Corresponding command to be execute and monitored for <see cref="ICommand.CanExecuteChanged" />
		/// </summary>
		public ICommand Command
		{
			get { return _command; }
			set
			{
				if (_command != null)
				{
					_command.CanExecuteChanged -= _commandCanExecuteChangedHandler;
				}

				_command = value;
				if (_command != null)
				{
					_command.CanExecuteChanged += _commandCanExecuteChangedHandler;
					UpdateEnabledState();
				}
			}
		}

		/// <summary>
		/// 	The parameter to supply the command during execution
		/// </summary>
		public object CommandParameter
		{
			get { return _commandParameter; }
			set
			{
				if (_commandParameter != value)
				{
					_commandParameter = value;
					UpdateEnabledState();
				}
			}
		}

		/// <summary>
		/// 	Confirmation.
		/// </summary>
		public bool IsConfirm { get; set; }

		/// <summary>
		/// 	ConfirmCaption.
		/// </summary>
		public string ConfirmCaption { get; set; }

		/// <summary>
		/// 	ConfirmText.
		/// </summary>
		public string ConfirmMessage { get; set; }

		/// <summary>
		/// 	Object to which this behavior is attached.
		/// </summary>
		protected T TargetObject
		{
			get { return _targetObject.Target as T; }
		}

		/// <summary>
		/// 	Updates the target object's IsEnabled property based on the commands ability to execute.
		/// </summary>
		protected virtual void UpdateEnabledState()
		{
			if (TargetObject == null)
			{
				Command = null;
				CommandParameter = null;
			}
			else if (Command != null)
			{
				TargetObject.IsEnabled = Command.CanExecute(CommandParameter);
			}
		}

		/// <summary>
		/// 	Executes the command, if it's set, providing the <see cref="CommandParameter" />
		/// </summary>
		protected virtual void ExecuteCommand()
		{
			if (Command == null)
				return;

			if (IsConfirm)
			{
				_messageWindow.Title = ConfirmCaption;
				_messageWindow.Message = ConfirmMessage;
				_messageWindow.Show();
				return;
			}

			Command.Execute(CommandParameter);
		}

		private void CommandCanExecuteChanged(object sender, EventArgs e)
		{
			UpdateEnabledState();
		}

		private void MessageWindowClosed(object sender, EventArgs e)
		{
			var dialogResult = _messageWindow.DialogResult;
			if (!dialogResult.HasValue || !dialogResult.Value)
				return;

			Command.Execute(CommandParameter);
		}
	}
}