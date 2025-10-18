using a2p.Application.DTOs;
using a2p.Application.Interfaces.Excel;
using a2p.Application.Models;

namespace a2p.Infrastructure.Services.ExcelServices
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