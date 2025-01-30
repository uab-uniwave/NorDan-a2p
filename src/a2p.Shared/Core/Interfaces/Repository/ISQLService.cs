using System.Data;

using Microsoft.Data.SqlClient;

namespace a2p.Shared.Core.Interfaces.Repository
{
 public interface ISqlService
 {


  Task<DataTable> ExecuteQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);
  Task<int> ExecuteNonQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);
  Task<object> ExecuteScalarAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);

  Task<(T1, T2)> ExecuteQueryForTwoValuesAsync<T1, T2>(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);

  //Task<string> GetColorAsync(string order);

  //  Task<string> GetConnectionAsync(bool useOleProvider = false);
 }
}
