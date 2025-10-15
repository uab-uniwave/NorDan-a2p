using a2p.Application.Models;

namespace a2p.Application.Interfaces
{
    public interface IExcelWriterService
    {

        void WriteExcelErrorLog(string file);
        //  Task<List<AppWorksheet>> GetWorksheetListAsync(List<OrderEntry> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<Worksheet>> GetWorksheetsAsync(Models.File file, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }



}
