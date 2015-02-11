using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.MsAccess.Types
{
	public struct DbType : IDbType
	{
		/// <summary>
		/// Creates NVarchar
		/// </summary>
		/// <param name="length">Length of NVarchar.</param>
		/// <param name="dbNull">Is Null.</param>
		public DbType(int length, DbNull dbNull = DbNull.NotNull)
		{
			_length = length;
			_dbNull = dbNull;
			_isPrimary = false;
			_type = DbTypes.NVARCHAR;
		}

		public DbType(DbTypes type, DbNull dbNull = DbNull.NotNull, bool isPrimary = false)
		{
			Guard.CheckFalse(type == DbTypes.NVARCHAR, "NVarchar should be created using another constructor.");
			Guard.CheckFalse(dbNull == DbNull.Null && isPrimary, "PRIMARY KEY cannot be null.");

			_type = type;
			_dbNull = dbNull;
			_isPrimary = isPrimary;
			_length = 0;
		}

		public override string ToString()
		{
			var command = _type == DbTypes.NVARCHAR
				? "{0}".FormatString(new Varchar(_length, _dbNull))
				: "{0}".FormatString(_type).ToUpperInvariant();

			command = "{0} {1}".FormatString(command, _dbNull == DbNull.NotNull ? "NOT NULL" : "NULL");
			command = "{0} {1}".FormatString(command, _isPrimary ? "PRIMARY KEY" : string.Empty);

			return command.Trim();
		}

		private readonly DbNull _dbNull;
		private readonly bool _isPrimary;
		private readonly int _length;
		private readonly DbTypes _type;
	}
}