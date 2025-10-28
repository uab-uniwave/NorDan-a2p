using Application.DTOs;
using Application.Interfaces.Orchestrators;
using Application.Interfaces.PrefSuite;
using Application.Interfaces.Services;
using Application.Models;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Orchestartors
{
    public class WriteService : IWriteService
    {
        private readonly ILogger<WriteService> _logger;

        private readonly IPrefSuiteAppService _prefSuiteService;
        //  private readonly IItemService _itemService;
        //  private readonly IMaterialService _materialService;
        private readonly IOrderService _orderService;

        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;

        public WriteService(ILogger<WriteService> logger,
                            IPrefSuiteAppService prefSuiteService,
                            //      IFileService fileService,
                            //      IExcelService excelService,
                            //         IPrefSuiteDataService prefSuiteDataService,
                            //         IItemService itemService,
                            //         IMaterialService materialService,
                            IOrderService orderService
            )
        {

            _logger = logger;
            _prefSuiteService = prefSuiteService;
            //      _itemService = itemService;
            //      _materialService = materialService;
            _orderService = orderService;

            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }

        public async Task WriteAsync(OrderDto orderDto, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            _progressValue = progressValue;
            _progress = progress ?? _progress;

            try
            {
                _progressValue.CurrentValue = _progressValue.CurrentValue + 30;   //30pts. x 1 per OrderNumber               
                _progressValue.ProgressTask2 = "Deleting any existing data import pending order.....";
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);

                await _prefSuiteService.InsertItemsAsync(orderDto, _progressValue, _progress);
                await _orderService.CreateOrderAsync(orderDto);

                //for (int i = 0; i < order.MaterialsDto.Count; i++)
                //{
                //    try
                //    {
                //        _progressValue.CurrentValue = _progressValue.CurrentValue + 1;//1pts. x 1 per material
                //        _progressValue.ProgressTask2 = $"Inserting materials {i + 1} of {order.MaterialsDto.Count} into PrefSuite DB...";
                //        _progressValue.ProgressTask3 = $"Material # {order.MaterialsDto[i].ReferenceBase} {order.MaterialsDto[i].Color}.";
                //        _progress?.Report(_progressValue);
                //       // await _materialService.CreateMaterialAsync(order.MaterialsDto[i]);
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError(ex, "Error while inserting material at index {Index}", i);
                //    }
                //}

                //_progressValue.CurrentValue = _progressValue.CurrentValue + 30; //30 pts x 2  per OrderNumber
                //_progressValue.ProgressTask2 = $"Inserting material needs in to PrefSuite... ";
                //_progressValue.ProgressTask3 = string.Empty;
                //_progress?.Report(_progressValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in WriteAsync");
            }
        }
    }
}
