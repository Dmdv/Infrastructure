using System.Data.OleDb;
using System.IO;
using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.MsAccess
{
	public class MdbTable
	{
		private const string ConnectionStringFormat = "Provider=Microsoft.JET.OLEDB.4.0;data source={0};";

		public string MdbFile { get; private set; }

		protected string SubKey { get; private set; }

		protected OleDbConnection CreateConnection()
		{
			return new OleDbConnection(ConnectionStringFormat.FormatString(MdbFile));
		}

		protected MdbTable(string file)
		{
			Guard.CheckTrue(
				File.Exists(file),
				() => new FileNotFoundException("MS Access file was not found: '{0}'.".FormatString(file)));

			MdbFile = file;
			SubKey = Path.GetFileName(MdbFile);
		}
	}
}