using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services.Other
{
 public interface IExcelService
 {
  //  Task<List<AppWorksheet>> GetWorksheetListAsync(List<OrderEntry> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
  Task<List<A2PWorksheet>> GetWorksheetListAsync(List<A2PFile> a2tpFile, ProgressValue progressValue, IProgress<ProgressValue>? progress);

 }
}
