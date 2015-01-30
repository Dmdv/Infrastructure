using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using Castle.Windsor;
using Common.Annotations;
using Net.Common.Contracts;
using Net.Common.Extensions;
using Net.Common.Monads;
using Net.WindsorCommon.Extensions;

namespace Net.AdoNetCommon.Extensions
{
	public static class AdapterExtenions
	{
		private const int CommandTimeout = 300;

		[PublicAPI]
		public static TAdapter ApplyCommands<TAdapter>(
			[NotNull] this TAdapter adapter,
			[NotNull] Action<SqlCommand> action)
			where TAdapter : Component, IDisposable
		{
			Guard.CheckNotNull(adapter, "adapter");
			Guard.CheckNotNull(action, "action");

			var property = GetCommandCollectionProperty(adapter);

			property.IfNotNull(prop =>
			{
				var commands = prop
					.GetValue(adapter, null)
					.Cast<IEnumerable>()
					.OfType<SqlCommand>();

				commands.ForEach(action);
			});

			return adapter;
		}

		[PublicAPI]
		public static void Execute(this SqlConnection connection, Action action)
		{
			Guard.CheckNotNull(connection, "connection");
			Guard.CheckNotNull(action, "action");

			connection.OpenIfClosed();
			action();
		}

		[PublicAPI]
		public static void ExecuteAutoOpenClose(this SqlConnection connection, Action action)
		{
			Guard.CheckNotNull(connection, "connection");
			Guard.CheckNotNull(action, "action");

			connection.OpenIfClosed();

			try
			{
				action();
			}
			finally
			{
				connection.Close();
			}
		}

		[PublicAPI]
		public static TReturn ExecuteAutoOpenClose<TReturn>(this SqlConnection connection, Func<TReturn> action)
		{
			Guard.CheckNotNull(connection, "connection");
			Guard.CheckNotNull(action, "action");

			connection.OpenIfClosed();

			try
			{
				return action();
			}
			finally
			{
				connection.Close();
			}
		}

		[PublicAPI]
		public static void FillTable<TAdapter, TTable>(this TAdapter adapter, TTable dataTable)
			where TAdapter : Component, IDisposable
			where TTable : DataTable
		{
			Guard.CheckNotNull(adapter, "adapter");
			Guard.CheckNotNull(dataTable, "dataTable");

			dynamic proxy = adapter;

			using (adapter)
			{
				var propertyInfo = GetConnectionProperty(adapter);

				if (propertyInfo == null)
				{
					proxy.Fill(dataTable);
				}
				else
				{
					var connection = propertyInfo.GetValue(adapter, null).Cast<SqlConnection>();
					connection.ExecuteAutoOpenClose(() => proxy.Fill(dataTable));
				}
			}
		}

		/// <summary>
		/// Registers all adapters from DataSet using OnCreateComponentDesciptor.
		/// </summary>
		/// <typeparam name="TInterface">A type which namespace is used in search.</typeparam>
		[PublicAPI]
		public static void RegisterAdaptersInNamespaceAs<TInterface>(this IWindsorContainer container)
			where TInterface : class
		{
			Guard.CheckNotNull(container, "container");

			var basedOnDescriptor = container.ConfigureInNamespaceAs<TInterface>();

			basedOnDescriptor
				.ConfigureFor<Component>(x =>
					x.AddDescriptor(
						new OnCreateComponentDescriptor<Component>((kernel, item) =>
						{
							var sqlConnection = kernel.Resolve<SqlConnection>();
							item.SetupConnection(sqlConnection);
						})));

			container.Register(basedOnDescriptor);
		}

		[PublicAPI]
		public static void RegisterAdaptersInNamespaceByName<TInterface>(
			this IWindsorContainer container,
			Predicate<Type> predicate)
			where TInterface : class
		{
			Guard.CheckNotNull(container, "container");

			var basedOnDescriptor = container
				.ConfigureInNamespaceAs<TInterface>()
				.If(predicate);

			basedOnDescriptor
				.ConfigureFor<Component>(x =>
					x.OnCreate(adapter =>
					{
						var sqlConnection = container.Resolve<SqlConnection>();
						adapter.Cast<Component>().SetConnection(sqlConnection);
					}));
		}

		[PublicAPI]
		public static void SetConnection<TAdapter>(
			this TAdapter adapter,
			SqlConnection connection)
			where TAdapter : Component
		{
			Guard.CheckNotNull(adapter, "adapter");
			Guard.CheckNotNull(connection, "connection");

			var propertyInfo = GetConnectionProperty(adapter);

			if (propertyInfo != null)
			{
				propertyInfo.SetValue(adapter, connection, null);
			}
		}

		[PublicAPI]
		public static TAdapter SetupConnection<TAdapter>(
			this TAdapter adapter,
			SqlConnection connection)
			where TAdapter : Component
		{
			Guard.CheckNotNull(adapter, "adapter");
			Guard.CheckNotNull(connection, "connection");

			adapter.SetConnection(connection);

			var property = GetCommandCollectionProperty(adapter);

			if (property != null)
			{
				var commands = property
					.GetValue(adapter, null)
					.Cast<IEnumerable>()
					.OfType<SqlCommand>();

				// ReSharper disable once PossibleMultipleEnumeration
				adapter.InitCommands(x => commands, connection);
			}

			return adapter;
		}

		private static PropertyInfo GetCommandCollectionProperty<TAdapter>(TAdapter adapter)
			where TAdapter : Component, IDisposable
		{
			Guard.CheckNotNull(adapter, "adapter");

			return
				adapter
					.GetType()
					.GetProperty("CommandCollection", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		}

		private static PropertyInfo GetConnectionProperty<TAdapter>(TAdapter adapter)
			where TAdapter : Component, IDisposable
		{
			Guard.CheckNotNull(adapter, "adapter");

			return
				adapter
					.GetType()
					.GetProperty("Connection", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		}

		[PublicAPI]
		private static TAdapter InitCommands<TAdapter>(
			this TAdapter adapter,
			Func<TAdapter, IEnumerable<IDbCommand>> selector,
			SqlConnection sqlConnection = null)
			where TAdapter : Component
		{
			Guard.CheckNotNull(adapter, "adapter");
			Guard.CheckNotNull(selector, "selector");

			var sqlCommands = selector(adapter).ToArray();

			sqlCommands.ForEach(command => command.CommandTimeout = CommandTimeout);
			sqlConnection.Do(connection => sqlCommands.ForEach(command => command.Connection = connection));

			return adapter;
		}

		private static void OpenIfClosed(this SqlConnection connection)
		{
			Guard.CheckNotNull(connection, "connection");

			connection
				.IfNot(x => x.State == ConnectionState.Open)
				.Do(x => x.Open());
		}
	}
}