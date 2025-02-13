using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Services;

using System.Data;


namespace a2p.Shared.Infrastructure.Services
{
    public class PrefService : IPrefService
    {
        private readonly ILogService _logService;
        private readonly ISqlRepoitory _sqlRepository;
        public PrefService(ILogService logService, ISqlRepoitory sqlService)
        {
            _logService = logService;
            _sqlRepository = sqlService;

        }


        public async Task<(int, int)> GetSalesDocAsync(string order)
        {
            try
            {

                string sqlCommand = "SELECT Numero, Version FROM PAF WHERE Referencia = " + "'" + order + "'";
                CommandType commandType = System.Data.CommandType.Text;
                (int, int) result = await _sqlRepository.ExecuteQueryTupleValuesAsync(sqlCommand, commandType);

                return result;


            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "PS: Unhandled error getting Order: {$Order} sales document number and version");
                return (-1, -1);
            }


        }
        public async Task<string?> MaterialsExistsAsync(string order)
        {
            try
            {
                string sqlCommand = $"SELECT MAX ([ModifiedUTCDateTime]) FROM  [dbo].[Uniwave_a2p_Materials] WHERE [Order] = '{order}' and [DeletedUTCDateTime] is null";
                object? result = await _sqlRepository.ExecuteScalarAsync(sqlCommand, CommandType.Text);
                return result.ToString();
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "PrefSuite Service:: Unhandled error while checking materials for Order: {$Order}");
                return null;
            }
        }



        public async Task<string?> ItemsExistsAsync(string order)
        {
            try
            {
                string sqlCommand = $"SELECT MAX ([ModifiedUTCDateTime]) FROM  [dbo].[Uniwave_a2p_Items] WHERE [Order] = '{order}' and [DeletedUTCDateTime] is null";
                object? result = await _sqlRepository.ExecuteScalarAsync(sqlCommand, CommandType.Text);
                return result.ToString();
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "PrefSuite Service: Unhandled error while checking Items for Order: {$Order}");
                return null;
            }
        }

        //public async Task<DateTime?> MaterialsExistsAsync(string order)
        //{
        //    try
        //    {
        //        if (order == null)
        //        {
        //            _logService.Error("WS: Error checking materials in DB. Order is null");
        //            return null;
        //        }

        //        SqlCommand cmd = new()
        //        {
        //            CommandText = "[dbo].[Uniwave_a2p_MaterialsExists]",
        //            CommandType = CommandType.StoredProcedure
        //        };

        //        SqlParameter orderParam = new("@Order", SqlDbType.NVarChar, 50)
        //        {
        //            Value = order
        //        };

        //        SqlParameter outputParam = new("@ModifiedUTCDateTime", SqlDbType.DateTime)
        //        {
        //            Direction = ParameterDirection.Output
        //        };

        //        object result = await _sqlRepository.ExecuteScalarAsync("[dbo].[Uniwave_a2p_MaterialsExists]", CommandType.StoredProcedure, orderParam, outputParam);

        //        if (result != null)
        //        {
        //            DateTime modifiedDate = (DateTime)result;

        //            if (outputParam.Value != DBNull.Value)
        //            {
        //                _logService.Warning($"WS: Order: {order} materials already imported on {outputParam.Value}.");
        //                return (DateTime?)outputParam.Value;
        //            }




        //        }
        //        _logService.Debug($"WS: Did not find any existing materials for Order: {order}.");
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.Error(ex, $"WS: Unhandled error while checking materials for Order: {order}");
        //        return null;
        //    }
        //}
        //public async Task<DateTime?> ItemsExistsAsync(string order)
        //{
        //    try
        //    {
        //        if (order == null)
        //        {
        //            _logService.Error("WS: Error checking items in DB. Order is null");
        //            return null;
        //        }

        //        SqlCommand cmd = new()
        //        {
        //            CommandText = "[dbo].[Uniwave_a2p_ItemsExists]",
        //            CommandType = CommandType.StoredProcedure
        //        };
        //        _ = cmd.Parameters.AddWithValue("@Order", order);

        //        SqlParameter outputParam = new("@ModifiedUTCDateTime", SqlDbType.DateTime)
        //        {
        //            Direction = ParameterDirection.Output
        //        };
        //        _ = cmd.Parameters.Add(outputParam);

        //        // Execute the command and read the output parameter
        //        _ = await _sqlRepository.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

        //        if (outputParam.Value != DBNull.Value)
        //        {
        //            _logService.Warning($"WS: Order: {order} items already imported on {outputParam.Value}.");
        //            return (DateTime?)outputParam.Value;
        //        }

        //        _logService.Debug($"WS: Did not find any existing items for Order: {order}.");
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.Error(ex, $"WS: Unhandled error while checking items for Order: {order}");
        //        return null;
        //    }
        //}
        public async Task<string?> GetColorAsync(string color)
        {
            try
            {
                string sqlCommand = "SELECT Color FROM Colors WHERE Color = '" + color + "'";
                CommandType commandType = System.Data.CommandType.Text;
                object result = await _sqlRepository.ExecuteScalarAsync(sqlCommand, commandType);

                return result?.ToString();
            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: getting color from DB");
                return null;
            }
        }


    }


}