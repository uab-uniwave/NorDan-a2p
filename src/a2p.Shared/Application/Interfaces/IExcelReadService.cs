using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Services.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IExcelReadService
    {
        //  Task<List<AppWorksheet>> GetWorksheetListAsync(List<OrderEntry> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<A2PWorksheet>> GetWorksheetsAsync(A2PFile file, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }
}