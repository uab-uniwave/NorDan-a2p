using Application.Interfaces;
using Application.Models;

using Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

using System.Data;


namespace Infrastructure.Data
    {
        /// <summary>
        /// Wrapper service for Dapper operations with logging and error handling
        /// </summary>
        public class DapperService
        {
            private readonly IDbConnectionFactory _connectionFactory;
            private readonly ILogger<DapperService> _logger;

            public DapperService(
                IDbConnectionFactory connectionFactory,
                ILogger<DapperService> logger)
            {
                _connectionFactory = connectionFactory;
                _logger = logger;
            }

            /// <summary>
            /// Execute a query and return a single result or default
            /// </summary>
            public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null)
            {
                try
                {
                    using IDbConnection connection = _connectionFactory.CreateConnection();
                    _logger.LogDebug("Executing QuerySingleOrDefault: {Sql}", sql);
                    return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing QuerySingleOrDefault: {Sql}", sql);
                    throw;
                }
            }

            /// <summary>
            /// Execute a query and return multiple results
            /// </summary>
            public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
            {
                try
                {
                    using IDbConnection connection = _connectionFactory.CreateConnection();
                    _logger.LogDebug("Executing Query: {Sql}", sql);
                    return await connection.QueryAsync<T>(sql, param);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing Query: {Sql}", sql);
                    throw;
                }
            }

            /// <summary>
            /// Execute a command (INSERT, UPDATE, DELETE) and return rows affected
            /// </summary>
            public async Task<int> ExecuteAsync(string sql, object? param = null)
            {
                try
                {
                    using IDbConnection connection = _connectionFactory.CreateConnection();
                    _logger.LogDebug("Executing Command: {Sql}", sql);
                    return await connection.ExecuteAsync(sql, param);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing Command: {Sql}", sql);
                    throw;
                }
            }

            /// <summary>
            /// Execute a scalar query (e.g., COUNT, SUM)
            /// </summary>
            public async Task<T> ExecuteScalarAsync<T>(string sql, object? param = null)
            {
                try
                {
                
                    using IDbConnection connection = _connectionFactory.CreateConnection();
                    _logger.LogDebug("Executing Scalar: {Sql}", sql);
                     
                var result = await connection.ExecuteScalarAsync<T>(sql, param);
                if (result == null)
                {
                    _logger.LogWarning("ExecuteScalar returned null for SQL: {Sql}", sql);
                    throw new InvalidOperationException($"ExecuteScalar returned null for SQL: {sql}");
                }

                return result;
                
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing Scalar: {Sql}", sql);
                    throw;
                }
            }

            // (disposing the GridReader will also close/dispose the underlying connection).
            public async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object? param = null)
            {

                try
                {
                    using IDbConnection connection = _connectionFactory.CreateConnection();
                    _logger.LogDebug("Executing Scalar: {Sql}", sql);
                    // QueryMultipleAsync returns a GridReader that keeps a reference to the connection.
                    // Caller must Dispose() the returned GridReader when done to close the connection.
                    return await connection.QueryMultipleAsync(sql, param);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing Scalar: {Sql}", sql);
                    throw;
                }
            }
        }
    }
