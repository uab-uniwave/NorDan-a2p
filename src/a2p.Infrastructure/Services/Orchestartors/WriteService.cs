
using a2p.Application.DTOs; // adjust if OrderDto lives elsewhere
using a2p.Application.Interfaces.Excel;
using a2p.Application.Interfaces.Files;
using a2p.Application.Interfaces.Orchestrators;
using a2p.Application.Interfaces.PrefSuite;
using a2p.Application.Models;

using Microsoft.Extensions.Logging;

namespace a2p.Infrastructure.Services.Orchestartors
{
    public class WriteService : IWriteService
    {
        private readonly ILogger<WriteService> _logger;
        private readonly IExcelService _excelService;
        private readonly IFileService _fileService;
        private readonly IPrefSuiteDataService _prefSuiteDataService;
        private readonly IPrefSuiteService _prefSuiteService;

        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;

        public WriteService(ILogger<WriteService> logger,
                            IPrefSuiteService prefSuiteService,
                            IFileService fileService,
                            IExcelService excelService,
                            IPrefSuiteDataService prefSuiteDataService

            )
        {

            _prefSuiteDataService = prefSuiteDataService;
            _fileService = fileService;
            _excelService = excelService;
            _logger = logger;
            _prefSuiteService = prefSuiteService;

            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }

        public async Task WriteAsync(OrderDto orders, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            _progressValue = progressValue;
            _progress = progress ?? _progress;

            try
            {
                _progressValue.CurrentValue = _progressValue.CurrentValue + 30;   //30pts. x 1 per Order               
                _progressValue.ProgressTask2 = "Deleting any existing data import pending orders.....";
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);

                await _prefSuiteService.InsertItemsAsync(orders, _progressValue, _progress);

                for (int i = 0; i < orders.ItemsDto.Count; i++)
                {
                    try
                    {
                        _progressValue.CurrentValue = _progressValue.CurrentValue + 10;  //10pts. x 1 per ItemName
                        _progressValue.ProgressTask2 = $"Inserting items {i + 1} of {orders.ItemsDto.Count} into db...";
                        _progressValue.ProgressTask3 = $"ItemName # {orders.ItemsDto[i].ItemName}";

                        _progress?.Report(_progressValue);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "ErrorDto while inserting item at index {Index}", i);
                    }
                }

                for (int i = 0; i < orders.MaterialsDto.Count; i++)
                {
                    try
                    {
                        _progressValue.CurrentValue = _progressValue.CurrentValue + 1;//1pts. x 1 per material
                        _progressValue.ProgressTask2 = $"Inserting materials {i + 1} of {orders.MaterialsDto.Count} into PrefSuite DB...";
                        _progressValue.ProgressTask3 = $"Material # {orders.MaterialsDto[i].ReferenceBase} {orders.MaterialsDto[i].Color}.";
                        _progress?.Report(_progressValue);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "ErrorDto while inserting material at index {Index}", i);
                    }
                }

                _progressValue.CurrentValue = _progressValue.CurrentValue + 30; //30 pts x 2  per OrderNumber
                _progressValue.ProgressTask2 = $"Inserting material needs in to PrefSuite... ";
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in WriteAsync");
            }
        }
    }
}
