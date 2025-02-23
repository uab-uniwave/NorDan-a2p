using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public class OrderWritingProcessor : IOrderWriteProcessor
    {
        private readonly ILogService _logService;

        private readonly IMapperSapaV1 _mapperSapaV1;
        private readonly IMapperSapaV2 _mapperSapaV2;
        private readonly IMapperSchuco _mapperSchuco;
        private readonly IWriteMaterialService _writeMaterialService;
        private readonly IWriteItemService _writeItemService;
        private readonly IPrefSuiteService _prefSuiteService;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public OrderWritingProcessor(ILogService logService,

            IWriteItemService writeItemsService,
            IWriteMaterialService writeMaterialService,

              IMapperSapaV1 mapperSapaV1,
              IMapperSapaV2 mapperSapaV2,
              IMapperSchuco mapperSchuco,
              IPrefSuiteService prefSuiteService)

        {
            _logService = logService;
            _mapperSapaV1 = mapperSapaV1;
            _mapperSapaV2 = mapperSapaV2;
            _mapperSchuco = mapperSchuco;
            _prefSuiteService = prefSuiteService;

            _writeMaterialService = writeMaterialService;
            _writeItemService = writeItemsService;
            _progressValue = new ProgressValue();

        }

        public async Task WriteAsync(List<A2POrder> orders, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress ?? new Progress<ProgressValue>();

            try
            {

                _progressValue.ProgressTask1 = $"Processing Files...";
                _progress?.Report(_progressValue);

                DateTime now = DateTime.UtcNow;

                int orderCount = 0;
                foreach (A2POrder order in orders)
                {

                    _progressValue.ProgressTask1 = $"Importing Order# {order.Order} Order {orderCount}  of {orders.Count}";
                    _progress?.Report(_progressValue);

                    if (!order.Items.Any() || !order.Materials.Any())
                    {
                        _logService.Error("Mapping handler service: Error mapping data.Order {$Order}, No Items or Material to write into DB! ", order.Order);
                        continue;
                    }

                    _progressValue.ProgressTask1 = $"Deleting existing data... ";
                    _progress?.Report(_progressValue);

                    if (order.OverwriteOrder == true)
                    {
                        _progressValue.ProgressTask1 = $"Deleting existing data... ";
                        _progress?.Report(_progressValue);

                        _ = await _writeItemService.DeleteAsync(order.Order); //TODO:  DeleteMaterialsAsync
                        _ = await _writeMaterialService.DeleteAsync(order.Order); //TODO:  DeleteMaterialsAsync
                        order.OverwriteOrder = false;
                        order.OrderExists = null;

                    }

                    _progressValue.ProgressTask1 = $"Data deleted...  ";

                    //Fort Each File in Order
                    //===================================================================================================================================
                    _progressValue.ProgressTask1 = $"Importing Order # {order.Order}.  {orderCount + 1} of {orders.Count()}";
                    _progress?.Report(_progressValue);

                    List<string?> result = await _prefSuiteService.InsertItemsAsync(order.Items, order.SalesDocNumber, order.SalesDocVersion, _progressValue, _progress);


                    _ = await _writeItemService.InsertListAsync(order.Items, order.SalesDocNumber, order.SalesDocVersion, _progressValue, _progress);

                    //Fort Each Material in Order
                    //===================================================================================================================================

                    _ = await _writeMaterialService.InsertListAsync(order.Materials, order.SalesDocNumber, order.SalesDocVersion, _progressValue, _progress);
                }
                orderCount++;
            }

            catch (Exception ex)
            {
                _logService.Error("Mapping handler service: Unhandled Error while mapping data. Exception: ${Exception} ", ex.Message);

            }

        }
    }
}
