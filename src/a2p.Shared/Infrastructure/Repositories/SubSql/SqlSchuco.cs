using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Repository.SubSql;
using a2p.Shared.Core.Interfaces.Services.Other;
using a2p.Shared.Core.Utils;

namespace a2p.Shared.Infrastructure.Repositories.SubSql
{
 public class SqlSchuco : ISqlSchuco
 {
  private readonly ISqlService _sqlService;
  private readonly ILogService _logger;

  public SqlSchuco(SqlService sqlService, ILogService logger, IPrefSuiteService prefService)
  {
   _sqlService=sqlService;
   _logger=logger;
  }


  public async Task<int> InsertPositionAsync(ItemDTO position)
  {


   return await Task.Run(() => 0);

  }
  public async Task<int> InsertMaterialAsync(MaterialDTO material)
  {
   return await Task.Run(() => 0);
  }

  public async Task<int> InsertGlassAsync(GlassDTO glass)
  {
   return await Task.Run(() => 0);
  }


 }
}
