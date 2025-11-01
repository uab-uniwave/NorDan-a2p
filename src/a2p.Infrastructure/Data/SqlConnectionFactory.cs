using Application.Interfaces;

using Microsoft.Data.SqlClient;

using System.Data;

namespace Infrastructure.Data
{
    /// <summary>
    /// Factory for creating SQL Server database connections
    /// </summary>
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString
            ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Creates a new SQL Server connection
        /// </summary>
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}