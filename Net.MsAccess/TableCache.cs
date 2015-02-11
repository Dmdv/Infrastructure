using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Common.Annotations;
using Net.Common.Contracts;
using Net.Common.Extensions;

namespace Net.MsAccess
{
	/// <summary>
	/// Thread safe cache.
	/// </summary>
	[PublicAPI]
	public static class TableCache
	{
		private static readonly object _lock = new object();
		private static readonly Dictionary<string, DataTable> _cache = new Dictionary<string, DataTable>();

		[PublicAPI]
		public static DataTable Get(TableKey tableKey)
		{
			lock (_lock)
			{
				return _cache.ContainsKey(tableKey.Value) ? _cache[tableKey.Value] : null;
			}
		}

		[PublicAPI]
		public static void Put(TableKey tableKey, DataTable datatable)
		{
			lock (_lock)
			{
				if (Get(tableKey) != null)
				{
					throw new Exception("'{0}' already exists".FormatString(tableKey));
				}
				_cache[tableKey.Value] = datatable;
			}
		}

		[PublicAPI]
		public static void Refresh(string path)
		{
			Guard.CheckTrue(
				Directory.Exists(path),
				() => new FileNotFoundException("Не найдена директория с данными: '{0}'".FormatString(path)));

			foreach (var file in Directory.GetFiles(path, @"*.mdb"))
			{
				var reader = new MdbReader(file);
				var schema = new SchemaBase(file);
				foreach (var tableName in schema.EnumerateTables())
				{
					Put(new TableKey(tableName, file), reader.FetchTable(tableName));
				}
			}
		}
	}
}