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
        /// Executes a SQL command and returns two values (document Order,document version)
        /// </summary>
        public async Task<(int, int)> ExecuteQueryTupleValuesAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters)
        {
            int value1;
            int value2;



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
                var result = await reader.ReadAsync();

                if (await reader.ReadAsync()) // Ensure we have data to read
                {
                    value1 = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                    value2 = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);

                }
                else
                {
                    value1 = -1;
                    value2 = -1;
                }





                return (value1, value2);

                // Return default values if no data is present
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "SS: Unhandled error Executing : {Order} sales document number and version");
                return (-1, -1);
            }
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
                foreach (var param in parameters)
                {
                    command.Parameters.Add(new SqlParameter(param.ParameterName, param.Value)
                    {
                        SqlDbType = param.SqlDbType,
                        Direction = param.Direction,
                        Size = param.Size
                    });
                }
            }

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return result;
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

