using Application.DTOs;
using Application.Interfaces.Excel;
using Application.Models;

namespace Infrastructure.Services.ExcelServices
{
    public class ExcelParserSchuco : IExcelParserSchuco
    {

        public Task<List<ItemDto>> MapItemsAsync(WorksheetDto worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<MaterialDto>> MapMaterialsAsync(WorksheetDto worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

    }
}