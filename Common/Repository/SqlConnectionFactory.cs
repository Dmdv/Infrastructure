using System;
using System.Data.SqlClient;

namespace Common.Repository
{
    public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        public SqlConnectionFactory(IConnectionSettings connectionSettings)
        {
            _connectionSettings = connectionSettings;
        }

        public void Dispose()
        {
            _sqlConnection.Dispose();
        }

        public SqlConnection CreateConnection()
        {
            _sqlConnection = new SqlConnection(_connectionSettings.ConnectionString);
            return _sqlConnection;
        }

        private readonly IConnectionSettings _connectionSettings;
        private SqlConnection _sqlConnection;
    }
}