// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Models;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Shared.Infrastructure.Services
{
    public class SQLService : ISQLService
    {
        private readonly IUserSettingsService _userSettingsService;
        private SettingsContainer _settingsContainer;
        private AppSettings _appSettings;
        private readonly string _connectionString;

        private readonly ILogService _logService;

        public SQLService(IUserSettingsService userSettingsService, ILogService logService)
        {
            try
            {
                _logService = logService ?? throw new ArgumentNullException(nameof(logService));

                if (userSettingsService == null)
                {
                    _logService.Error("UserSettingsService is null", "SQL Repository: Unhandled error initializing SQL Repository. Please check Settings.");
                    throw new ArgumentNullException(nameof(userSettingsService));
                }

                _userSettingsService = userSettingsService;

                _appSettings = _userSettingsService.LoadSettings();

                if (_appSettings == null)
                {
                    _logService.Error("AppSettings is null", "SQL Repository: Unhandled error initializing SQL Repository. Please check Settings.");
                    throw new ArgumentNullException(nameof(_appSettings));
                }

                _settingsContainer = _userSettingsService.LoadAllSettings();

                if (_settingsContainer == null)
                {
                    _logService.Error("SettingsContainer is null", "SQL Repository: Unhandled error initializing SQL Repository. Please check Settings.");
                    throw new ArgumentNullException(nameof(_settingsContainer));
                }
                _connectionString = _settingsContainer.ConnectionStrings["DefaultConnection"] ?? string.Empty;
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _logService.Error("ConnectionString is null or empty", "SQL Repository: Unhandled error initializing SQL Repository. Please check Settings.");
                    throw new ArgumentNullException(nameof(_connectionString));
                }

            }
            catch (Exception ex)
            {
                _logService = logService;
                _logService.Error(ex.Message, "SQL Repository: Unhandled error initializing SQL Repository. Please check Settings.");
            }
        }
        /// <summary>
        /// Executes a SQL command and returns a DataTable (useful for SELECT queries).
        /// </summary>
        public async Task<DataTable> ExecuteQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters)
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

                return dataTable;
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "SQL Repository: Unhandled error Executing query {$sqlCommand}", sqlCommand);
                return new DataTable();
            }
        }
        /// <summary>
        /// Executes a SQL command and returns two values (document Order,document version)
        /// </summary>
        public async Task<(int, int)> ExecuteQueryTupleValuesAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters)
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
                _logService.Verbose(
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
        public async Task<int> ExecuteNonQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters)
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
                _logService.Error(ex.Message, "SQL Repository: Unhandled error Executing non query {$sqlCommand}", sqlCommand);
                return -1;
            }
        }
        /// </summary>
        public async Task<object?> ExecuteScalarAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters)
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
                object? result = await command.ExecuteScalarAsync();
                return result ?? DBNull.Value;
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "SQL Repository: Unhandled error Executing scalar query {$sqlCommand}. Exception{$Exception}", sqlCommand, ex.Message);
                return DBNull.Value;
            }
        }
        /// <summary>
        /// Executes a stored procedure and returns a DataTable (useful for SELECT queries).
        /// </summary>
        public async Task<DataTable> ExecuteStoredProcedureAsync(string storedProcedureName, params SqlParameter[] parameters)
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
                _logService.Error(ex.Message, "SQL Repository: Unhandled error executing stored procedure {storedProcedureName}", storedProcedureName);
                return new DataTable();
            }
        }

    }
}
