using System;
using System.Collections.Generic;
using System.Data;
using Common.Annotations;
using Net.Common.Contracts;
using Net.Common.Extensions;
using Net.MsAccess.Extensions;

namespace Net.MsAccess
{
	[PublicAPI]
	public class TableCacheReader : TableReader
	{
		public TableCacheReader(string file)
			: base(file)
		{
		}

		[PublicAPI]
		public override DataTable FetchTable(string table)
		{
			Guard.CheckContainsText(table, "table");

			var datatable = TableCache.Get(new TableKey(table, SubKey));
			if (datatable == null)
			{
				datatable = base.FetchTable(table);
				TableCache.Put(new TableKey(table, SubKey), datatable);
			}
			return datatable;
		}

		[PublicAPI]
		public IEnumerable<string[]> SelectAllRows(string table)
		{
			Guard.CheckContainsText(table, "table");

			return FetchTableSafe(table).SelectAllRows();
		}

		[PublicAPI]
		public EnumerableRowCollection<T> SelectColumnRange<T>(string table, string column)
		{
			Guard.CheckContainsText(table, "table");
			Guard.CheckContainsText(column, "column");

			return FetchTableSafe(table).SelectColumnRange<T>(column);
		}

		[PublicAPI]
		public EnumerableRowCollection<T> SelectColumnRange<T>(string table, int columnIndex)
		{
			Guard.CheckContainsText(table, "table");

			return FetchTableSafe(table).SelectColumnRange<T>(columnIndex);
		}

		[PublicAPI]
		public Dictionary<string, string> SelectRowDictionary(string table, int index)
		{
			Guard.CheckContainsText(table, "table");

			return FetchTableSafe(table).SelectRowStringDictionary(index);
		}

		[PublicAPI]
		public string[] SelectRowRange(string table, int index)
		{
			Guard.CheckContainsText(table, "table");

			return FetchTableSafe(table).SelectRowRange(index);
		}

		[PublicAPI]
		private DataTable FetchTableSafe(string table)
		{
			Guard.CheckContainsText(table, "table");

			var dataTable = FetchTable(table);
			if (dataTable == null)
			{
				throw new Exception("Table '{0}' was  not found".FormatString(table));
			}
			return dataTable;
		}
	}
}