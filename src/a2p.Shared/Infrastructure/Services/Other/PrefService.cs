using a2p.Shared.Core.Interfaces.Repository.SubSql;
using a2p.Shared.Core.Interfaces.Services.Other;
using a2p.Shared.Core.Utils;


namespace a2p.Shared.Infrastructure.Services.Other
{
 public class PrefService : IPrefSuiteService
 {
  private readonly ILogService _logger;
  private readonly ISqlSapa_v2 _sqlSapaV2;
        public PrefService(ILogService logger,ISqlSapa_v2 sqlSapaV2)
  {
   _logger=logger;
   _sqlSapaV2 = sqlSapaV2;  



  }


  public (int, int) GetSalesDocNumber(string order)
  {
   _logger.Information("GetSalesDocNumber");

      // _sqlSapaV2.
            //     HeaderEncodingSelector<>


            return (1, 1);

  }


 }
}