using Application.DTOs;
using Application.Interfaces.Orchestrators;
using Application.Interfaces.PrefSuite;
using Application.Interfaces.Services;
using Application.Models;

using Domain.Entities;
using Domain.Shared;

using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Orchestartors
{
    public class WriteService : IWriteService
    {
        private readonly ILogger<WriteService> _logger;

        private readonly IPrefSuiteAppService _prefSuiteService;
        private readonly IPrefSuiteDataService _prefSuiteDataService;
        // private readonly IItemService _itemService;
        // private readonly IMaterialService _materialService;
        private readonly IOrderService _orderService;

        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;

        public WriteService(ILogger<WriteService> logger,
         IPrefSuiteAppService prefSuiteService,
         IPrefSuiteDataService prefSuiteDataService,
         IOrderService orderService)
        {

            _logger = logger;
            _prefSuiteService = prefSuiteService;
            _orderService = orderService;
            _prefSuiteDataService = prefSuiteDataService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }

        public async Task WriteAsync(OrderDto orderDto, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            _progressValue = progressValue;
            _progress = progress ?? _progress;

            try
            {
                _progressValue.CurrentValue = _progressValue.CurrentValue + 30; //30pts. x 1 per OrderNumber 
                _progressValue.ProgressTask2 = "Deleting any existing data import pending order.....";
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);

                await _orderService.DeleteOrderByIdAsync(orderDto.Id);

                await _prefSuiteService.InsertItemsAsync(orderDto, _progressValue, _progress);
                ValidationResult<OrderEntity> orderEntity = await _orderService.CreateOrderAsync(orderDto);

                if (!orderEntity.IsSuccess || orderEntity.Value == null)
                {
                    _logger.LogError("Failed to create order entity for OrderNumber {OrderNumber}", orderDto.OrderNumber);
                    return;
                }

                OrderEntity order = orderEntity.Value;

                List<MaterialEntity> materials = order.Materials;

                for (int i = 0; i < materials.Count; i++)
                {

                    progressValue.CurrentValue = _progressValue.CurrentValue + 1;//1pts. x 1 per material
                    _progressValue.ProgressTask2 = $"Inserting materials {i + 1} of {materials.Count} into PrefSuite DB...";
                    _progressValue.ProgressTask3 = $"Material # {materials[i].ReferenceBase} {materials[i].Color}.";
                    _progress?.Report(_progressValue);

                    await _prefSuiteDataService.InsertPrefSuiteColorAsync(materials[i]);
                    await _prefSuiteDataService.InsertPrefSuiteColorConfigurationAsync(materials[i]);
                    await _prefSuiteDataService.InsertPrefSuiteMaterialBaseAsync(materials[i]);
                    await _prefSuiteDataService.InsertPrefSuiteMaterialAsync(materials[i]);
                    await _prefSuiteDataService.InsertPrefSuiteMaterialProfileAsync(materials[i]);
                    await _prefSuiteDataService.InsertPrefSuiteMaterialMeterAsync(materials[i]);
                    await _prefSuiteDataService.InsertPrefSuiteMaterialPieceAsync(materials[i]);
                    await _prefSuiteDataService.InsertPrefSuiteMaterialSurfaceAsync(materials[i]);
                    await _prefSuiteDataService.InsertPrefSuiteMaterialPurchaseDataAsync(materials[i]);
                    await _prefSuiteDataService.UpdateBCMapping(materials[i]);
                }

                _progressValue.CurrentValue = _progressValue.CurrentValue + 30; //30 pts x 2 per OrderNumber
                _progressValue.ProgressTask2 = $"Inserting material needs in to PrefSuite... ";
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);

                if (order.OrderNumber == null || order.SalesDocumentNumber == 0 || order.SalesDocumentVersion == 0)
                {
                    _logger.LogError("SalesDocumentNumber or SalesDocumentVersion is zero for OrderNumber {OrderNumber}", order.OrderNumber);
                    return; ;
                }
                await _prefSuiteDataService.InsertPrefSuiteMaterialNeedsMasterAsync(order.Id);
                await _prefSuiteDataService.InsertPrefSuiteMaterialNeedsAsync(order.Id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in WriteAsync");
            }
        }
    }
}
