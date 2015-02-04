using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace EditorClient.Commands
{
	public class DelegateCommand<T> : ICommand
	{
		private readonly Func<T, bool> _canExecute;
		private readonly Action<T> _executeMethod;
		private List<WeakReference> _canExecuteChangedHandlers;

		public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecute = null)
		{
			if (executeMethod == null)
			{
				throw new ArgumentNullException("executeMethod");
			}
			_executeMethod = executeMethod;
			_canExecute = canExecute;
		}

		public event EventHandler CanExecuteChanged
		{
			add { WeakEventHandlerManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2); }
			remove { WeakEventHandlerManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value); }
		}


		void ICommand.Execute(object parameter)
		{
			Execute((T)parameter);
		}

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute((T)parameter);
		}

		public bool CanExecute(T parameter)
		{
			var result = true;
			var canExecuteHandler = _canExecute;
			if (canExecuteHandler != null)
			{
				result = canExecuteHandler(parameter);
			}

			return result;
		}

		public void Execute(T parameter)
		{
			_executeMethod(parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			WeakEventHandlerManager.CallWeakReferenceHandlers(this, _canExecuteChangedHandlers);
		}
	}
}