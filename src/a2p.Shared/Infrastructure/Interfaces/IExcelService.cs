using a2p.Shared.Application.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IExcelService
    {

             void WriteExcelErrorLog(string file, List<A2PError> A2PError);
        //  Task<List<AppWorksheet>> GetWorksheetListAsync(List<OrderEntry> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<A2PWorksheet>> GetWorksheetsAsync(A2PFile file, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }



}
