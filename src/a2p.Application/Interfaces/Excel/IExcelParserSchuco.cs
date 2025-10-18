using a2p.Application.DTOs;
using a2p.Application.Models;

namespace a2p.Application.Interfaces.Excel
{
    public interface IExcelParserSchuco
    {

        Task<List<ItemDto>> MapItemsAsync(WorksheetDto worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDto>> MapMaterialsAsync(WorksheetDto worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}