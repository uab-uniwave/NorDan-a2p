using a2p.Application.DTOs;
using a2p.Application.Models;

namespace a2p.Application.Interfaces
{
    public interface IExcelParserSchuco
    {

        Task<List<ExcelItemDto>> MapItemsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<ExcelMaterialDto>> MapMaterialsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}