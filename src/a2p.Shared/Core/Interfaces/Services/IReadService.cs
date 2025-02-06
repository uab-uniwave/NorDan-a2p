using a2p.Shared.Core.Entities.Models;

namespace a2p.Shared.Core.Interfaces.Services
{
    public interface IReadService
    {
        //  Task<List<AppWorksheet>> GetWorksheetListAsync(List<OrderEntry> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<A2POrderFileWorksheet>> GetWorksheetListAsync(List<A2POrderFile> a2tpFile, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }
}
