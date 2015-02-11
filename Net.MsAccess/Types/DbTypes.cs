using System;

namespace Net.MsAccess.Types
{
	// ReSharper disable InconsistentNaming

	[Flags]
	public enum DbTypes : uint
	{
		TINYINT = 0,
		AUTOINCREMENT,
		MONEY,
		DATETIME,
		UNIQUEIDENTIFIER,
		DECIMAL,
		REAL,
		FLOAT,
		SMALLINT,
		INTEGER,
		IMAGE,
		NVARCHAR
	}
}