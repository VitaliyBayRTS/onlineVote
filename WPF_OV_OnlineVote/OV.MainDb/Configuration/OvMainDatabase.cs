using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace OV.MainDb.Configuration
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
    public class OvMainDatabase : IDbConnectionFactory
    {
        private string _connectionString { get; set; } = "Data Source=DESKTOP-O2K81VH;Initial Catalog=OV_MainDb;Integrated Security=True";
        private int _environmentId;
        private DbContextOptions<OvMainDbContext> options;

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
