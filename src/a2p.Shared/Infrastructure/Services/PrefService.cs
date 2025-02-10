using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Services;

using Microsoft.Data.SqlClient;

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
                var commandType = System.Data.CommandType.Text;
                var result = await _sqlRepository.ExecuteQueryTupleValuesAsync(sqlCommand, commandType);

                return result;


            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "PS: Unhandled error getting Order: {$Order} sales document number and version");
                return (-1, -1);
            }


        }

        public async Task<DateTime?> MaterialsExistsAsync(string order)
        {
            try
            {
                if (order == null)
                {
                    _logService.Error("WS:Error deleting (Update DeletedUTCDateTime) materials from DB. Order is null");
                    return null;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_MaterialsExists]",
                    CommandType = CommandType.StoredProcedure
                };
                _ = cmd.Parameters.AddWithValue("@Order", order);

                var outputParam = new SqlParameter("@ModifiedUTCDateTime", SqlDbType.DateTime)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                var result = await _sqlRepository.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());



                if (!string.IsNullOrEmpty(result.ToString()))
                {
                    _logService.Warning("WS: Order: {$Order} materials already imported on.", order);
                    return (DateTime?)outputParam.Value;
                }
                _logService.Debug("WS: Did not found any existing materials of Order: {$Order}.", order);
                return null;

            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "WS: Unhandled error: deleting Sapa v2 order: {$Order} materials from DB", order);
                return null;
            }
        }
        public async Task<DateTime?> ItemsExistsAsync(string order)
        {
            try
            {
                if (order == null)
                {
                    _logService.Error("WS:Error deleting (Update DeletedUTCDateTime) items from DB. Order is null");
                    return null;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_ItemsExists]",
                    CommandType = CommandType.StoredProcedure
                };
                _ = cmd.Parameters.AddWithValue("@Order", order);

                var outputParam = new SqlParameter("@ModifiedUTCDateTime", SqlDbType.DateTime)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                var result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Warning("WS: Order: {$Order} materials already imported on.", order);
                    return (DateTime?)outputParam.Value;
                }

                _logService.Debug("WS: Did not found any existing materials of Order: {$Order}.", order);
                return null;
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "WS: Unhandled error: deleting Sapa v2 order: {$Order} materials from DB", order);
                return null;
            }
        }
        public async Task<string?> GetColorAsync(string color)
        {
            try
            {
                string sqlCommand = "SELECT Color FROM Colors WHERE Color = '" + color + "'";
                var commandType = System.Data.CommandType.Text;
                var result = await _sqlRepository.ExecuteScalarAsync(sqlCommand, commandType);

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