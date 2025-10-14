using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Application.Interfaces
{
    public interface ISQLService
    {


        Task<DataTable?> ExecuteQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters);
        Task<int> ExecuteNonQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters);
        Task<DataTable> ExecuteScalarAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters);

        Task<(int, int)?> ExecuteQueryTupleValuesAsync(string sqlCommand, CommandType commandType, params SqlParameter[]? parameters);


    }
}
