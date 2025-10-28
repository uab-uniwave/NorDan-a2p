using Application.DTOs;
using Application.Interfaces.Excel;
using Application.Models;

namespace Infrastructure.Services.ExcelServices
{
    public class ExcelParserSchuco : IExcelParserSchuco
    {

        public Task<List<ItemDto>> MapItemsAsync(Worksheet worksheet, OrderDto orderDto, ProgressValue? progressValue = null, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<MaterialDto>> MapMaterialsAsync(Worksheet worksheet, OrderDto orderDto, ProgressValue? progressValue = null, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

    }
}