using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Services;


namespace a2p.Shared.Infrastructure.Services
{
    public class PrefService : IPrefService
    {
        private readonly ILogService _logService;
        private readonly ISqlRepoitory _sqlService;
        public PrefService( ILogService logService, ISqlRepoitory sqlService)
        {
            _logService = logService;
            _sqlService = sqlService;

        }


        public async Task<(int, int)> GetSalesDocAsync(string order)
        {
            try
            {

                string sqlCommand = "SELECT Numero, Version FROM PAF WHERE Referencia = " + "'" + order + "'";
                var commandType = System.Data.CommandType.Text;
                var result = await _sqlService.ExecuteQueryTupleValuesAsync<int, int>(sqlCommand, commandType);

                return result;


            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "PS: Unhandled error getting OrderNumber: {$OrderNumber} sales document number and version");
                return (0, 0);
            }


        }


        public async Task<string?> GetColorAsync(string color)
        {
            try
            {
                string sqlCommand = "SELECT Color FROM Colors WHERE Color = '" + color + "'";
                var commandType = System.Data.CommandType.Text;
                var result = await _sqlService.ExecuteScalarAsync(sqlCommand, commandType);

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