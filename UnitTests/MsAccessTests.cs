using System;
using System.Data.OleDb;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Common.Contracts;
using Net.Common.Extensions;
using Net.MsAccess;
using Net.MsAccess.Types;

namespace UnitTests
{
	[TestClass]
	public class MsAccessTests
	{
		[TestMethod]
		public void TestCreateMdb()
		{
			try
			{
				try
				{
					if (File.Exists("ConfigStructure.mdb"))
					{
						File.Delete("ConfigStructure.mdb");
					}

					File.Copy(@"Templates\Empty.mdb", "ConfigStructure.mdb");
				}
				catch (Exception ex)
				{
					throw new Exception("File operation exception", ex);
				}

				const string StrTemp = " [KEY] Text, [VALUE] Text ";
				using (var connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=ConfigStructure.mdb"))
				{
					connection.Open();

					var myCommand = new OleDbCommand
					{
						Connection = connection,
						CommandText = "CREATE TABLE table1(" + StrTemp + ")"
					};

					myCommand.ExecuteNonQuery();
				}
			}
			catch (Exception)
			{
				File.Delete("ConfigStructure.mdb");
			}
		}

		[TestMethod]
		public void TestCommandNotNullAndPrimaryKey()
		{
			new DbType(DbTypes.INTEGER).ToString().Should().Be("INTEGER NOT NULL");
			new DbType(DbTypes.INTEGER, DbNull.Null).ToString().Should().Be("INTEGER NULL");
			new DbType(DbTypes.INTEGER, DbNull.NotNull, true).ToString().Should().Be("INTEGER NOT NULL PRIMARY KEY");
		}

		[TestMethod]
		public void TestCommands()
		{
			new DbType(DbTypes.INTEGER).ToString().Should().Be("INTEGER NOT NULL");
			new DbType(DbTypes.REAL).ToString().Should().Be("REAL NOT NULL");
			new DbType(DbTypes.AUTOINCREMENT).ToString().Should().Be("AUTOINCREMENT NOT NULL");
			new DbType(DbTypes.DATETIME).ToString().Should().Be("DATETIME NOT NULL");
			new DbType(DbTypes.DECIMAL).ToString().Should().Be("DECIMAL NOT NULL");
			new DbType(DbTypes.FLOAT).ToString().Should().Be("FLOAT NOT NULL");
			new DbType(DbTypes.IMAGE).ToString().Should().Be("IMAGE NOT NULL");
			new DbType(DbTypes.MONEY).ToString().Should().Be("MONEY NOT NULL");
			new DbType(DbTypes.SMALLINT).ToString().Should().Be("SMALLINT NOT NULL");
			new DbType(DbTypes.TINYINT).ToString().Should().Be("TINYINT NOT NULL");
			new DbType(DbTypes.UNIQUEIDENTIFIER).ToString().Should().Be("UNIQUEIDENTIFIER NOT NULL");
		}

		[TestMethod]
		public void TestNVarChar()
		{
			var varchar = new Varchar(40);
			varchar.ToString().Should().Be("VARCHAR(40) NOT NULL");
			varchar = new Varchar(50, DbNull.Null);
			varchar.ToString().Should().Be("VARCHAR(50) NULL");
		}

		[TestMethod]
		[ExpectedException(typeof (ContractViolationException))]
		public void TestNullPrimaryKey()
		{
			new DbType(DbTypes.INTEGER, DbNull.Null, true).Should();
		}

		[TestMethod]
		public void TestWrite()
		{
			var mdbWriter = MdbWriter.Create("testMdb.mdb");
			mdbWriter.Update("testtable", new Tuple<string, double>("col1", 1.1d).YieldArray(), 1);
		}
	}
}