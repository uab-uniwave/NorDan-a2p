using a2p.Domain.Entities;

namespace a2p.Infrastructure.Services
{
    public interface IExcelService
    {

        void WriteExcelErrorLog(string file, List<ErrorEntity> Error);
        //  Task<List<AppWorksheet>> GetWorksheetListAsync(List<OrderEntry> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<Worksheet>> GetWorksheetsAsync(File file, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }



}
