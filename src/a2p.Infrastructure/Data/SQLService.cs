using Application.Interfaces.Services;
using Application.Models;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using System.Data;
namespace Infrastructure.Data
{
    public class SQLService : ISQLService
    {
        private readonly ISettingsService _settingsService;
        private SettingsContainer _settingsContainer;
        private readonly string _connectionString;

        private readonly ILogger<SQLService> _logger;

        public SQLService(ISettingsService settingsService, ILogger<SQLService> logger)
        {
            try
            {
                _logger = logger;
                _settingsService = settingsService;
                _settingsContainer = _settingsService.GetSettings();
                _connectionString = _settingsContainer.ConnectionStrings["DefaultConnection"] ?? string.Empty;


            }
            catch (Exception ex)
            {
                _logger = logger;
                _logger.LogError(ex.Message, "SQL Repository: Unhandled error initializing SQL Repository. Please check Settings.");
            }
        }
        /// <summary>
        /// Executes a SQL command and returns a DataTable (useful for SELECT queries).
        /// </summary>
        public async Task<DataTable?> ExecuteQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters)
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

                    foreach (SqlParameter param in parameters)
                    {
                        _ = command.Parameters.Add(new SqlParameter(param.ParameterName, param.Value)
                        {
                            SqlDbType = param.SqlDbType,
                            Direction = param.Direction,
                            Size = param.Size
                        });
                    }
                }

                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();

                DataTable dataTable = new();
                dataTable.Load(reader);

                return dataTable.Rows.Count == 0 ? null : dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "SQL Repository: Unhandled error Executing query {$sqlCommand}", sqlCommand);
                return null;
            }
        }
        /// <summary>
        /// Executes a SQL command and returns two values (document OrderNumber,document version)
        /// </summary>
        public async Task<(int, int)> ExecuteQueryTupleValuesAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters)
        {
            int value1;
            int value2;
            (int, int) result = (-1, -1);

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
                bool readerResult = await reader.ReadAsync();
                if (readerResult)
                {
                    value1 = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                    value2 = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                    result = (value1, value2);
                }
                return result;

                // Return default values if no data is present
            }
            catch (Exception ex)
            {
                _logger.LogError(
           "{$Class}.{$Method}. Unhandled error getting result from method. sException: {Exception}.",
           nameof(SQLService),
           nameof(ExecuteQueryTupleValuesAsync),
            ex.Message
          );

                return result;
            }
        }

        /// <summary>
        /// Executes a SQL command that does not return data (useful for INSERT, UPDATE, DELETE, etc.).
        /// </summary>
        public async Task<int> ExecuteNonQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters)
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
                    foreach (SqlParameter param in parameters)
                    {
                        _ = command.Parameters.Add(new SqlParameter(param.ParameterName, param.Value)
                        {
                            SqlDbType = param.SqlDbType,
                            Direction = param.Direction,
                            Size = param.Size
                        });
                    }
                }

                await connection.OpenAsync();
                int result = await command.ExecuteNonQueryAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "SQL Repository: Unhandled error Executing non query {$sqlCommand}", sqlCommand);
                return -1;
            }
        }
        /// </summary>
        public async Task<DataTable?> ExecuteScalarAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters)
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

                    foreach (SqlParameter param in parameters)
                    {
                        _ = command.Parameters.Add(new SqlParameter(param.ParameterName, param.Value)
                        {
                            SqlDbType = param.SqlDbType,
                            Direction = param.Direction,
                            Size = param.Size
                        });
                    }
                }

                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();

                DataTable dataTable = new();
                dataTable.Load(reader);

                return dataTable.Rows.Count == 0 ? null : dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "SQL Repository: Unhandled error Executing scalar query {$sqlCommand}. Exception{$Exception}", sqlCommand, ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Executes a stored procedure and returns a DataTable (useful for SELECT queries).
        /// </summary>
        public async Task<DataTable> ExecuteStoredProcedureAsync(string storedProcedureName, params SqlParameter[]? parameters)
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(storedProcedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (parameters != null)
                {

                    foreach (SqlParameter param in parameters)
                    {
                        _ = command.Parameters.Add(new SqlParameter(param.ParameterName, param.Value)
                        {
                            SqlDbType = param.SqlDbType,
                            Direction = param.Direction,
                            Size = param.Size
                        });
                    }
                }

                await connection.OpenAsync();
                using SqlDataReader reader = await command.ExecuteReaderAsync();

                DataTable dataTable = new();
                dataTable.Load(reader);

                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "SQL Repository: Unhandled error executing stored procedure {storedProcedureName}", storedProcedureName);
                return new DataTable();
            }
        }

    }
}
