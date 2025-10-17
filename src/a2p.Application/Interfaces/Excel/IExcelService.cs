using a2p.Application.DTOs;
using a2p.Application.Models;

namespace a2p.Application.Interfaces.Excel
{
    public interface IExcelService
    {

        void WriteExcelErrorLog(string file, List<ErrorEntity> errorDto);
        // Task<List<Worksheet>> GetWorksheetListAsync(List<ExcelOrderDto> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<Worksheet>> GetWorksheetsAsync(Models.File file, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }



}
