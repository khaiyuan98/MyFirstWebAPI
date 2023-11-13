using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Test.DataAccess.Data
{
    public class SqlDataAccess : ISqlDataAccess
    {
        public readonly string connectionString;

        public SqlDataAccess(IConfiguration config, string connectionId)
        {
            connectionString = config.GetConnectionString(connectionId);
        }

        public SqlDataAccess(IConfiguration config)
        {
            connectionString = config.GetConnectionString("Default");
        }

        public IDbConnection OpenConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
