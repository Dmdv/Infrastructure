using System;
using System.Linq;
using System.Collections.ObjectModel;
using EditorClient.Commands;
using EditorClient.Helper;
using EditorClient.ServiceReference;
using EditorClient.Services;
using OnlineEditor.Service;

namespace EditorClient.ViewModels
{
	/// <summary>
	/// View Model for documents.
	/// </summary>
	public class DocumentListVm : BaseViewModel
	{
		private ObservableCollection<DocumentVm> _documents;
		private DocumentVm _selectedDocument;

		private int _selectedIndex;

		public DocumentListVm()
		{
			Documents = new ObservableCollection<DocumentVm>();
			CreateCommand = new DelegateCommand<DocumentVm>(Create, CanCreate);
			LoadCommand = new DelegateCommand<DocumentVm>(Load, CanLoad);
			DeleteCommand = new DelegateCommand<DocumentVm>(Delete, CanDelete);
			
			if (IsInDesignMode)
			{
				Documents.Add(new DocumentVm(new Document {Name = "doc1"}));
				Documents.Add(new DocumentVm(new Document {Name = "doc2"}));
			}
			else
			{
				Scheduler.RegisterTask(UpdateList, 1000);
			}
		}

		public DelegateCommand<DocumentVm> CreateCommand { get; set; }
		public DelegateCommand<DocumentVm> DeleteCommand { get; set; }
		public DelegateCommand<DocumentVm> LoadCommand { get; set; }

		public ObservableCollection<DocumentVm> Documents
		{
			get { return _documents; }
			set
			{
				_documents = value;
				OnPropertyChanged("Documents");
			}
		}

		public int SelectedIndex
		{
			get { return _selectedIndex; }
			set
			{
				_selectedIndex = value;
				OnPropertyChanged("SelectedIndex");
			}
		}

		public DocumentVm SelectedDocument
		{
			get { return _selectedDocument; }
			set
			{
				_selectedDocument = value;
				OnPropertyChanged("SelectedDocument");
				LoadCommand.RaiseCanExecuteChanged();
				DeleteCommand.RaiseCanExecuteChanged();
				CreateCommand.RaiseCanExecuteChanged();
			}
		}

		public void Delete(DocumentVm obj)
		{
			Dispatcher.BeginInvoke(new Action(DeleteDocument));
		}

		private bool CanDelete(DocumentVm obj)
		{
			return Service.Online && SelectedDocument != null;
		}

		private void DeleteDocument()
		{
			var result = Service.Proxy.Delete(SelectedDocument.Name, Service.CurrentUser);
			if (!result.Success)
			{
				const string Msg = "Failed to delete document\r\nMessage: {0};\r\nState: {1}";
				Logger.LogError(string.Format(Msg, result.Buffer, result.State));
			}
			else
			{
				SelectedDocument.Delete();
				Documents.Remove(SelectedDocument);
			}
		}

		private void Load(DocumentVm doc)
		{
			SelectedDocument.Load();
		}

		private bool CanLoad(DocumentVm doc)
		{
			return Service.Online && SelectedDocument != null;
		}

		private void Create(DocumentVm doc)
		{
			var result = Service.Proxy.Create(null, Service.CurrentUser);
			if (!result.Success)
			{
				const string Msg = "Failed to create document\r\nMessage: {0};\r\nState: {1}";
				Logger.LogError(string.Format(Msg, result.Buffer, result.State));
			}
		}

		private bool CanCreate(DocumentVm doc)
		{
			return Service.Online;
		}

		private void UpdateList()
		{
			if (!Service.Online) return;
			Dispatcher.BeginInvoke(new Action(UpdateListInternal));
		}

		private void UpdateListInternal()
		{
			var documents = Service.Proxy.AvailableDocuments().Select(x => new DocumentVm(x)).ToList();
			if (Documents.SequenceEqual(documents, new DocumentStatefulComparer())) return;

			if (SelectedDocument != null)
			{
				_selectedIndex = documents.Contains(SelectedDocument, new DocumentComparer())
				                 	? Documents.IndexOf(SelectedDocument)
				                 	: 0;
			}

			foreach (var document in Documents)
			{
				document.Delete();
			}

			Documents = new ObservableCollection<DocumentVm>(documents);
			foreach (var doc in Documents.Where(x=>x.IsEagerLoading))
			{
				doc.Load();
			}

			OnPropertyChanged("SelectedIndex");
		}
	}
}
