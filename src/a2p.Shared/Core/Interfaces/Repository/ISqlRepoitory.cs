using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Shared.Core.Interfaces.Repository
{
    public interface ISqlRepoitory
    {


        Task<DataTable> ExecuteQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);
        Task<int> ExecuteNonQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);
        Task<object> ExecuteScalarAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);

        Task<(T1, T2)> ExecuteQueryTupleValuesAsync<T1, T2>(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);


        //  Task<string> GetConnectionAsync(bool useOleProvider = false);
    }
}
