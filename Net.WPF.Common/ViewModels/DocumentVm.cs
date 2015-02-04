using System;
using EditorClient.Commands;
using EditorClient.ServiceReference;
using EditorClient.Services;
using OnlineEditor.Service;

namespace EditorClient.ViewModels
{
	/// <summary>
	/// View Model for document.
	/// </summary>
	public class DocumentVm : BaseViewModel
	{
		private int _taskId = -1;
		private bool _deleted;

		public DocumentVm(Document doc)
		{
			Document = doc;
			CloseCommand = new DelegateCommand<DocumentVm>(Close, CanClose);
		}

		public DelegateCommand<DocumentVm> CloseCommand { get; set; }

		public string Text
		{
			get { return Document.Text; }
			set
			{
				Document.Text = value;
				OnPropertyChanged("Text");
			}
		}

		public string Name
		{
			get { return Document.Name; }
			set
			{
				Document.Name = value;
				OnPropertyChanged("Name");
			}
		}

		public Document Document { get; private set; }

		public bool IsEagerLoading
		{
			get { return Document.ReadOnly && Document.Owner == Service.CurrentUser; }
		}

		public void Load()
		{
			Dispatcher.BeginInvoke(new Action(LoadDocument));
			_taskId = Scheduler.RegisterTask(Save, 1000);
		}

		public void Delete()
		{
			_deleted = true;
			_taskId = Scheduler.UnregisterTask(_taskId);
		}

		private void LoadDocument()
		{
			var result = Service.Proxy.Open(Name, Service.CurrentUser);
			if (result.Success)
			{
				Text = result.Buffer;
			}
			else
				switch (result.State)
				{
					case State.Opened:
						Logger.LogError(string.Format("{0} is opened for edit by another user", Name));
						break;
					case State.Deleted:
						Logger.LogError(string.Format("{0} was deleted by another user", Name));
						break;
					default:
						Text = "Failed to load text, Error: " + result.Buffer;
						break;
				}
		}

		private void CloseDocument()
		{
			var result = Service.Proxy.Close(Name, Service.CurrentUser);
			if (!result.Success)
			{
				const string Msg = "Failed to close document\r\nMessage: {0};\r\nState: {1}";
				Logger.LogError(string.Format(Msg, result.Buffer, result.State));
			}
			Text = string.Empty;
		}

		private void Close(DocumentVm obj)
		{
			Dispatcher.BeginInvoke(new Action(CloseDocument));
			_taskId = Scheduler.UnregisterTask(_taskId);
		}

		private bool CanClose(DocumentVm arg)
		{
			return Service.Online;
		}

		private void Save()
		{
			if (!CanSave()) return;

			var result = Service.Proxy.Write(Name, Service.CurrentUser, Text);
			if (!result.Success)
			{
				var msg = result.State == State.Deleted ? "Document deleted" : string.Empty;
				Logger.LogError(msg + "\r\n" + result.Buffer);
			}
		}

		private bool CanSave()
		{
			return 
				Service.Online && 
				!_deleted &&
				Document.ReadOnly && 
				Document.Owner == Service.CurrentUser && 
				Text != null;
		}
	}
}