using a2p.Shared.Core.Interfaces.Services.Other;
using a2p.Shared.Core.Utils;


namespace a2p.Shared.Infrastructure.Services.Other
{
 public class PrefService : IPrefSuiteService
 {
  private readonly ILogService _logger;
  public PrefService(ILogService logger)
  {
   _logger=logger;
  }


  public (int, int) GetSalesDocNumber(string order)
  {
   _logger.Information("GetSalesDocNumber");
   return (1, 1);

  }


 }
}