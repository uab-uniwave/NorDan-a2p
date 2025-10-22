using System.Data;

namespace Application.Interfaces
{
    /// <summary>
    /// Factory interface for creating database connections
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Creates and returns a new database connection
        /// </summary>
        IDbConnection CreateConnection();
    }
}