using Application.DTOs;
using Application.Models;

namespace Application.Interfaces.Excel
{
    public interface IExcelParserSchuco
    {

        Task<List<ItemDto>> MapItemsAsync(Worksheet worksheet, OrderDto orderDto, ProgressValue? progressValue=null, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDto>> MapMaterialsAsync(Worksheet worksheet, OrderDto orderDto, ProgressValue? progressValue = null, IProgress<ProgressValue>? progress = null);
    }
}