// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;

using Microsoft.Data.SqlClient;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface ISqlRepository
    {

        Task<DataTable> ExecuteQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);
        Task<int> ExecuteNonQueryAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);
        Task<object?> ExecuteScalarAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);

        Task<(int, int)> ExecuteQueryTupleValuesAsync(string sqlCommand, CommandType commandType, params SqlParameter[] parameters);

        //  Task<string> GetConnectionAsync(bool useOleProvider = false);
    }
}
