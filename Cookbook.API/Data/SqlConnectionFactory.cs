using Cookbook.API.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace Cookbook.API.Data
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(IOptions<DatabaseSettings> databaseSettings)
        {
            _connectionString = databaseSettings.Value.CookbookDb;
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
