using Dapper;

using Microsoft.Data.SqlClient;

namespace a2p.Infrastructure.Services.DataServices
{
    public class DapperService
    {

        private readonly string _connectionString;

        public DapperService(string connectionString)
        {

            _connectionString = connectionString;

        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null)
        {
            using SqlConnection connection = new(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            using SqlConnection connection = new(_connectionString);
            return await connection.QueryAsync<T>(sql, param);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            using SqlConnection connection = new(_connectionString);
            return await connection.ExecuteAsync(sql, param);
        }
    }
}
