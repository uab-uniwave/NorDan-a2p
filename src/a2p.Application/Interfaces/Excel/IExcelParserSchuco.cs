using Application.DTOs;
using Application.Models;

namespace Application.Interfaces.Excel
{
    public interface IExcelParserSchuco
    {

        Task<List<ItemDto>> MapItemsAsync(WorksheetDto worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDto>> MapMaterialsAsync(WorksheetDto worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}