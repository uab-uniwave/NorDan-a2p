using System.Data;

using Microsoft.Data.SqlClient;

namespace a2p.Application.Interfaces.Services
{
    public interface ISQLService
    {

        Task<DataTable?> ExecuteQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters);
        Task<int> ExecuteNonQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters);
        Task<DataTable> ExecuteScalarAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters);

        Task<(int, int)> ExecuteQueryTupleValuesAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters);

    }
}
