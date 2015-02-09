using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.MsAccess
{
	/// <summary>
	/// Database schema provider.
	/// </summary>
	public class SchemaBase
	{
		private const string Pattern = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}";
		private static readonly DbProviderFactory _factory = DbProviderFactories.GetFactory("System.Data.OleDb");

		public SchemaBase(string file)
		{
			Guard.CheckTrue(
				File.Exists(file),
				() => new FileNotFoundException("File not found: '{0}'".FormatString(file)));

			_file = file;
		}

		public IEnumerable<string> EnumerateTables()
		{
			DataTable userTables;

			using (var connection = _factory.CreateConnection())
			{
				Guard.CheckNotNull(connection, "connection");

				// ReSharper disable once PossibleNullReferenceException
				connection.ConnectionString = Pattern.FormatString(_file);

				// We only want user tables, not system tables
				var restrictions = new string[4];
				restrictions[3] = "Table";

				connection.Open();

				// Get list of user tables
				userTables = connection.GetSchema("Tables", restrictions);
			}

			return userTables.Rows
				.Cast<DataRow>()
				.Select(row => row[2].ToString())
				.ToList();
		}

		private readonly string _file;
	}
}