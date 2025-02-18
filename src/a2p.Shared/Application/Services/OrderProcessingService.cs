using a2p.Shared.Application.Interfaces;
using a2p.Shared.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Application.Services
{
    public class OrderProcessingService : IOrderProcessingService
    {
        private readonly ILogService _logService;

        private readonly IMapperSapaV1 _mapperSapaV1;
        private readonly IMapperSapaV2 _mapperSapaV2;
        private readonly IMapperSchuco _mapperSchuco;
        private readonly IWriteMaterialService _writeMaterialService;
        private readonly IWriteItemService _writeItemService;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue> _progress;
        private IPrefSuiteIntegrationService _prefService;


        public OrderProcessingService(ILogService logService,

            IWriteItemService writeItemsService,
            IWriteMaterialService writeMaterialService,

              IMapperSapaV1 mapperSapaV1,
              IMapperSapaV2 mapperSapaV2,
              IMapperSchuco mapperSchuco,
              IPrefSuiteIntegrationService prefService)

        {
            _logService = logService;
            _mapperSapaV2 = mapperSapaV2;
            _writeMaterialService = writeMaterialService;
            _writeItemService = writeItemsService;
            _progressValue = new ProgressValue();
            _prefService = prefService;

        }

        public async Task<A2POrder> MapDataAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress ?? new Progress<ProgressValue>();

            try
            {

                _progressValue.ProgressTask1 = $"Processing Files...";
                _progress?.Report(_progressValue);



                if (order.Files == null || !order.Files.Any())
                {
                    _logService.Error("Mapping handler service: Error mapping data.Order {$Order}, files not found! ", order.Order);
                    return order;
                }

                if (order.OverwriteOrder == true)
                {
                    _progressValue.ProgressTask2 = $"Deleting existing data... ";
                    _progress?.Report(_progressValue);


                    _ = await _writeItemService.DeleteAsync(order.Order); //TODO:  DeleteMaterialsAsync
                    _ = await _writeMaterialService.DeleteAsync(order.Order); //TODO:  DeleteMaterialsAsync
                    order.OverwriteOrder = false;
                    order.OrderExists = null;

                    _progressValue.ProgressTask2 = $"Data deleted...  ";
                    _progress?.Report(_progressValue);
                }



                //Fort Each File in Order
                //===================================================================================================================================
                int fileCount = 0;
                foreach (A2PFile file in order.Files)
                {

                    _progressValue.ProgressTask1 = $"Importing File {file.FileName}. File {fileCount + 1} of {order?.Files.Count}.";
                    _progressValue.ProgressTask2 = $"Processing Worksheets...";
                    _progress?.Report(_progressValue);


                    if (file.Worksheets == null || !file.Worksheets.Any())
                    {
                        _logService.Error("Mapping handler service: Error mapping data.Order {$Order}, file {$File} worksheets not found! ", order!.Order, file.FileName);
                        continue;
                    }
                    foreach (A2PWorksheet worksheet in file.Worksheets)
                    {
                        if (worksheet == null || worksheet.RowCount == 0)
                        {
                            _logService.Error("Mapping handler service: Error mapping data.Order {$Order}, file {$File} worksheet {$Worksheet} rows not found! ", order!.Order, file.FileName, worksheet!.Worksheet);
                            continue;
                        }
                        if (order!.SourceAppType == SourceAppType.SapaV1)
                        {
                            if (file.IsOrderItemsFile)
                            {
                                order = await _mapperSapaV2.MapItemsAsync(order, _progressValue, _progress);
                            }
                            else if (file.IsOrderItemsFile)
                            {
                                order = await _mapperSapaV2.MapMaterialsAsync(order, _progressValue, _progress);
                            }
                        }
                        else if (order.SourceAppType == SourceAppType.SapaV2)
                        {
                            if (file.IsOrderItemsFile)
                            {
                                order = await _mapperSapaV2.MapItemsAsync(order, _progressValue, _progress);
                            }
                            else if (file.IsOrderItemsFile)
                            {
                                order = await _mapperSapaV2.MapMaterialsAsync(order, _progressValue, _progress);
                            }
                        }
                        else if (order.SourceAppType == SourceAppType.Schuco)
                        {
                            if (file.IsOrderItemsFile)
                            {
                                order = await _mapperSchuco.MapItemsAsync(order, _progressValue, _progress);
                            }
                            else if (file.IsOrderItemsFile)
                            {
                                order = await _mapperSchuco.MapMaterialsAsync(order, _progressValue, _progress);
                            }
                        }
                        else
                        {
                            _logService.Error("Mapping handler service: Error mapping data.Order {$Order}, SourceAppType not found! ", order!.Order);


                        }
                    }
                }
                return order!;


            }
            catch (Exception ex)
            {
                _logService.Error("Mapping handler service: Unhandled Error while mapping data. Exception: ${Exception} ", ex.Message);
                return order;
            }



        }
    }
}
