using System.Data.SqlClient;

namespace Common.Repository
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}