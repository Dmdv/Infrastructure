using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using Common.Contracts;
using Common.Extensions.Monads;

namespace Common.Extensions
{
	public static class AdapterExtenions
	{
		private const int CommandTimeout = 300;

		public static void Execute(this SqlConnection connection, Action action)
		{
			Guard.CheckNotNull(connection, "connection");
			Guard.CheckNotNull(action, "action");

			connection
				.IfNot(x => x.State == ConnectionState.Open)
				.Do(x => x.Open());

			connection.Do(x => action());
		}

		public static void ExecuteAutoOpenClose(this SqlConnection connection, Action action)
		{
			Guard.CheckNotNull(connection, "connection");
			Guard.CheckNotNull(action, "action");

			connection
				.IfNot(x => x.State == ConnectionState.Open)
				.Do(x => x.Open())
				.ExecuteAndDispose(action);
		}

		//public static TReturn ExecuteAutoOpenClose<TReturn>(this SqlConnection connection, Func<TReturn> action)
		//{
		//	Guard.CheckNotNull(connection, "connection");
		//	Guard.CheckNotNull(action, "action");

		//	return connection
		//		.IfNot(x => x.State == ConnectionState.Open)
		//		.Do(x => x.Open())
		//		.ExecuteAndDispose(action);
		//}


		public static void FillTable<TAdapter, TTable>(this TAdapter adapter, TTable dataTable)
			where TAdapter : Component, IDisposable
			where TTable : DataTable
		{
			Guard.CheckNotNull(adapter, "adapter");
			Guard.CheckNotNull(dataTable, "dataTable");

			dynamic proxy = adapter;

			using (adapter)
			{
				using (proxy.Connection)
				{
					proxy.Connection.Open();
					proxy.Fill(dataTable);
				}
			}
		}

		public static TAdapter InitCommands<TAdapter>(
			this TAdapter adapter,
			IEnumerable<IDbCommand> commands)
			where TAdapter : Component
		{
			// ReSharper disable PossibleMultipleEnumeration
			Guard.CheckNotNull(adapter, "adapter");
			Guard.CheckNotNull(commands, "commands");

			foreach (var command in commands)
			{
				command.CommandTimeout = CommandTimeout;
			}

			return adapter;
			// ReSharper restore PossibleMultipleEnumeration
		}

		public static TAdapter InitCommands<TAdapter>(
			this TAdapter adapter,
			Func<TAdapter, IEnumerable<IDbCommand>> selector)
			where TAdapter : Component
		{
			Guard.CheckNotNull(adapter, "adapter");
			Guard.CheckNotNull(selector, "selector");

			var sqlCommands = selector(adapter);

			foreach (var command in sqlCommands)
			{
				command.CommandTimeout = CommandTimeout;
			}

			return adapter;
		}

		public static TAdapter WithConnection<TAdapter>(this TAdapter adapter, SqlConnection connection)
			where TAdapter : Component
		{
			Guard.CheckNotNull(adapter, "adapter");
			Guard.CheckNotNull(connection, "connection");

			dynamic proxy = adapter;
			proxy.Connection = connection;
			return adapter;
		}
	}
}