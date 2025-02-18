using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IExcelReadService
    {
        //  Task<List<AppWorksheet>> GetWorksheetListAsync(List<OrderEntry> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<A2POrder> GetWorksheetsAsync(A2POrder order, ProgressValue progressValue,IProgress<ProgressValue>? progress); 
    }
}
