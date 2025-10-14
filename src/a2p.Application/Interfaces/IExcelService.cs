using a2p.Domain.Entities;
using a2p.Domain.Models;

namespace a2p.Application.Interfaces
{
    public interface IExcelService
    {

        void WriteExcelErrorLog(string file, List<ErrorEntity> Error);
        //  Task<List<AppWorksheet>> GetWorksheetListAsync(List<OrderEntry> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<Worksheet>> GetWorksheetsAsync(Domain.Models.File file, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }



}
