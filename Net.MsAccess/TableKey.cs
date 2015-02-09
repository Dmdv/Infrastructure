using System.IO;
using Common.Annotations;
using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.MsAccess
{
	[PublicAPI]
	public class TableKey
	{
		public TableKey(string table, string file)
		{
			Guard.CheckContainsText(table, "table");
			Guard.CheckContainsText(file, "file");

			Table = table;
			_file = Path.GetFileName(file);
		}

		[PublicAPI]
		public string Table { get; private set; }

		[PublicAPI]
		public string Value
		{
			get { return "{0}|{1}".FormatString(Table, _file.ToLower()); }
		}

		public override string ToString()
		{
			return Value;
		}

		private readonly string _file;
	}
}