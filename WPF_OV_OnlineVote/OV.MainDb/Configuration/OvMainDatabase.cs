using System.Data;
using System.Data.SqlClient;

namespace OV.MainDb.Configuration
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
    public class OvMainDatabase : IDbConnectionFactory
    {
        private string _connectionString { get; set; }

        public OvMainDatabase()
        {
        }

        public OvMainDatabase(Microsoft.Extensions.Options.IOptions<ConnectionStringConfig> connectionStringConfig)
        {
            _connectionString = connectionStringConfig.Value.OvMainDb;
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

    }
}
