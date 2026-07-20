using System.Data;

namespace Cookbook.API.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
