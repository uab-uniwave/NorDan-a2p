using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Services.Import.SubServices;
using a2p.Shared.Core.Utils;




namespace a2p.Shared.Infrastructure.Services.Import.SubServices
{
 public class ImportSchuco : IImportSchuco
 {
  private readonly ILogService _logger;
  private readonly ISqlService _sqlService;




  public ImportSchuco(ILogService logger, ISqlService sqlService)
  {
   _logger=logger;
   _sqlService=sqlService;
  }
  public async Task<int> ImportPositionsAsync(List<ItemDTO> positions)
  {
   return await Task.Run(() => 0);
  }

  public async Task<int> ImportMaterialsAsync(List<MaterialDTO> materials)
  {
   return await Task.Run(() => 0);
  }



  public async Task<int> ImportGlassesAsync(List<GlassDTO> glasses)
  {
   return await Task.Run(() => 0);
  }






 }
}

