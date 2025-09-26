using a2p.Application.DTO;
using a2p.Domain.Entities;

namespace a2p.Application.Interfaces
{
    public interface IExcelService
    {

             void WriteExcelErrorLog(string file, List<A2PErrorDto> A2PError);
        //  Task<List<AppWorksheet>> GetWorksheetListAsync(List<OrderEntry> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<A2PWorksheet>> GetWorksheetsAsync(A2PFile file, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }



}
