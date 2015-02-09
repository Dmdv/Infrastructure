using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Annotations;
using Net.Common.Contracts;

namespace Net.MsAccess
{
	[PublicAPI]
	public class MdbProvider
	{
		public MdbProvider(string path)
		{
			Guard.CheckContainsText(path, "path");
			Guard.CheckTrue(File.Exists(path), () => new FileNotFoundException(path));

			_cacheReader = new TableCacheReader(path);
			Path = _cacheReader.MdbFile;

			Tables = new SchemaBase(Path).GetTableNames().ToList();
		}

		[PublicAPI]
		public string Path { get; private set; }

		[PublicAPI]
		public TableCacheReader Reader
		{
			get { return _cacheReader; }
		}

		[PublicAPI]
		public IReadOnlyList<string> Tables { get; private set; }

		[PublicAPI]
		public int RowCount(string table)
		{
			return Reader.FetchTable(table).Rows.Count;
		}

		private readonly TableCacheReader _cacheReader;
	}
}