using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.MsAccess.Types
{
	public struct Varchar : IDbType
	{
		public Varchar(int length, DbNull dbNull = DbNull.NotNull)
		{
			Guard.CheckTrue(length > 0, "Length should be greater than 0");

			_length = length;
			_dbNull = dbNull;
		}

		public override string ToString()
		{
			var isnull = _dbNull == DbNull.NotNull ? "NOT NULL" : "NULL";
			return "VARCHAR({0}) {1}".FormatString(_length, isnull);
		}

		private readonly DbNull _dbNull;
		private readonly int _length;
	}
}