namespace EditorClient.ViewModels
{
	/// <summary>
	/// ViewModels locator.
	/// </summary>
	public class ViewModelLocator
	{
		private static DocumentListVm _documentListVm;

		public static DocumentListVm DocumentListVm
		{
			get { return _documentListVm ?? (_documentListVm = new DocumentListVm()); }
		}
	}
}
