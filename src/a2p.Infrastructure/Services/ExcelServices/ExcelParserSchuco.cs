using a2p.Application.DTOs;
using a2p.Application.Interfaces;
using a2p.Application.Models;

namespace a2p.Infrastructure.Services.MappingService
{
    public class ExcelParserSchuco : IExcelParserSchuco
    {

        public Task<List<ExcelItemDto>> MapItemsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExcelMaterialDto>> MapMaterialsAsync(Worksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

    }
}