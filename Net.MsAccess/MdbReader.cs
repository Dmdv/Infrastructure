using System;
using System.Data;
using System.Data.OleDb;
using Net.Common.Extensions;

namespace Net.MsAccess
{
	public class MdbReader : MdbTable
	{
		public MdbReader(string file)
			: base(file)
		{
		}

		public virtual DataTable FetchTable(string table)
		{
			using (var connection = CreateConnection())
			{
				connection.Open();

				using (var cmd = new OleDbCommand("select * from {0}".FormatString(table))
				{
					Connection = connection
				})
				{
					using (var oleDbDataReader = cmd.ExecuteReader())
					{
						if (oleDbDataReader == null)
						{
							throw new ApplicationException(
								"Reader is null for 'select * from {0}'".FormatString(table));
						}

						using (var dataTable = new DataTable())
						{
							dataTable.Load(oleDbDataReader);
							return dataTable;
						}
					}
				}
			}
		}
	}
}