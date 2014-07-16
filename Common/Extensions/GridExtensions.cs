using System.Data;
using System.Windows.Forms;
using Common.Annotations;
using Common.Contracts;

namespace Common.Extensions
{
	public static class GridExtensions
	{
		[NotNull]
		public static T GetCurrentRowThrows<T>([NotNull] this DataGridView dataRow) where T : DataRow
		{
			Guard.CheckNotNull(dataRow, "dataRow");
			var currentRow  = Guard.GetNotNull(dataRow.CurrentRow, "dataRow.CurrentRow");
			var rowView = currentRow.DataBoundItem.Cast<DataRowView>();
			return rowView.Row.Cast<T>();
		}
	}
}