using System;
using System.Data.OleDb;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Common.Extensions;
using Net.MsAccess;

namespace UnitTests
{
	[TestClass]
	public class MsAccessDdlTests
	{
		private const string MdbFile = "sample.mdb";
		private const string TemplateFile = @"Templates\Empty.mdb";

		[ClassCleanup]
		public static void Cleanup()
		{
			try
			{
				if (File.Exists(MdbFile))
				{
					File.Delete(MdbFile);
				}
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[ClassInitialize]
		public static void Init(TestContext context)
		{
			try
			{
				if (File.Exists(MdbFile))
				{
					File.Delete(MdbFile);
				}
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}

			try
			{
				File.Copy(TemplateFile, MdbFile);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		[TestMethod]
		public void TestCreateTable()
		{
			const string TableName = "table1";
			const string TableColumns = " [KEY] Text, [VALUE] Text ";

			try
			{
				var connectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}".FormatString(MdbFile);

				using (var connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					var command = new OleDbCommand
					{
						Connection = connection,
						CommandText = "CREATE TABLE {0}({1})".FormatString(TableName, TableColumns)
					};

					command.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}

			var schema = new SchemaBase(MdbFile);
			schema.EnumerateTables().Should().Contain(TableName);
		}
	}
}