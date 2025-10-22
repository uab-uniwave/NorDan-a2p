using Application.Models;

using Domain.Entities;

namespace Application.Interfaces.Excel
{
    public interface IExcelService
    {

        void WriteExcelErrorLog(string file, List<ErrorEntity> errorDto);
        // Task<List<WorksheetDto>> GetWorksheetListAsync(List<ExcelOrderDto> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<WorksheetDto>> GetWorksheetsAsync(Models.FileDto file, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }

}
