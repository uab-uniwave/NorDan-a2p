using Application.Models;

namespace Application.Interfaces.Excel
{
    public interface IExcelService
    {

        Task<List<Worksheet>> ReadWorkbook(ExcelFile file, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }

}
