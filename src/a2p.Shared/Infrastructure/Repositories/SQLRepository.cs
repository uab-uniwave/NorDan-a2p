using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Services;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System.Data;

namespace a2p.Shared.Infrastructure.Repositories
{
    public class SQLRepository : ISqlRepoitory
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ILogService _logService;

        public SQLRepository(IConfiguration configuration, ILogService logService)
        {

            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _connectionString = _configuration.GetConnectionString("DefaultConnection")
                 ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
        /// <summary>
        /// Executes a SQL command and returns a DataTable (useful for SELECT queries).
        /// </summary>
        public async Task<DataTable> ExecuteQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(sqlCommand, connection)
            {
                CommandType = commandType
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            await connection.OpenAsync();
            using SqlDataReader reader = await command.ExecuteReaderAsync();

            DataTable dataTable = new();
            dataTable.Load(reader);

            return dataTable;
        }
        /// <summary>
        /// Executes a SQL command and returns two values (document OrderNumber,document version)
        /// </summary>
        public async Task<(T1, T2)> ExecuteQueryTupleValuesAsync<T1, T2>(string sqlCommand, CommandType commandType, params SqlParameter[] parameters)
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(sqlCommand, connection)
                {
                    CommandType = commandType
                };

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    // Read the first and second values and convert them to the specified types
                    T1? value1 = reader.IsDBNull(0) ? default! : reader.GetFieldValue<T1>(0);
                    T2? value2 = reader.IsDBNull(1) ? default! : reader.GetFieldValue<T2>(1);
                    return (value1, value2);
                }
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "SS: Unhandled error Executing : {OrderNumber} sales document number and version");
            }

            return (default!, default!);
        }






        /// <summary>
        /// Executes a SQL command that does not return data (useful for INSERT, UPDATE, DELETE, etc.).
        /// </summary>
        public async Task<int> ExecuteNonQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(sqlCommand, connection)
            {
                CommandType = commandType
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }
        /// </summary>
        public async Task<object> ExecuteScalarAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(sqlCommand, connection)
            {
                CommandType = commandType
            };

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            await connection.OpenAsync();
            object? result = await command.ExecuteScalarAsync();
            return result ?? throw new InvalidOperationException("Query did not return any result.");
        }


    }
}

