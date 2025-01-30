using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services.Import
{
 public interface IImportService
 {
  Task ImportDataAsync(IEnumerable<A2POrder> orderList, IProgress<ProgressValue>? progress = null);

 }
}
