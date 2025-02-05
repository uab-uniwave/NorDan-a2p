using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Services.Read;
using a2p.Shared.Core.Utils;




namespace a2p.Shared.Infrastructure.Services.Read
{
 public class ReadSchuco : IReadSchuco
 {
  private readonly ILogService _logger;
  private readonly ISqlService _sqlService;




  public ReadSchuco(ILogService logger, ISqlService sqlService)
  {
   _logger=logger;
   _sqlService=sqlService;
  }
  public async Task<int> ReadPositionsAsync(List<ItemDTO> positions)
  {
   return await Task.Run(() => 0);
  }

  public async Task<int> ReadMaterialsAsync(List<MaterialDTO> materials)
  {
   return await Task.Run(() => 0);
  }



  public async Task<int> ReadGlassesAsync(List<GlassDTO> glasses)
  {
   return await Task.Run(() => 0);
  }






 }
}

