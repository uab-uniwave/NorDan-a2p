// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Infrastructure.Services
{
    public class OrderWritingProcessor : IOrderWriteProcessor
    {
        private readonly ILogService _logService;
        private readonly IWriteMaterialService _writeMaterialService;
        private readonly IWriteItemService _writeItemService;
        private readonly IPrefSuiteService _prefSuiteService;
        private readonly DataCache _dataCache;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public OrderWritingProcessor(ILogService logService,

            IWriteItemService writeItemsService,
            IWriteMaterialService writeMaterialService,

              IMapperSapaV1 mapperSapaV1,
              IMapperSapaV2 mapperSapaV2,
              IMapperSchuco mapperSchuco,
              IPrefSuiteService prefSuiteService,
              DataCache dataCache)

        {
            _logService = logService;
            _prefSuiteService = prefSuiteService;
            _dataCache = dataCache;
            _writeMaterialService = writeMaterialService;
            _writeItemService = writeItemsService;
            _progressValue = new ProgressValue();

        }

        public async Task WriteAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress ?? new Progress<ProgressValue>();

            try
            {

                _progressValue.ProgressTask1 = $"Preparing data import...";
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);
                DateTime now = DateTime.UtcNow;

                List<A2POrder> a2pOrders = _dataCache.GetAllOrders();
                _progressValue.MinValue = 0;
                _progressValue.MaxValue = a2pOrders.Count() * 2;

                int orderCount = 0;
                foreach (A2POrder a2pOrder in a2pOrders)
                {
                    orderCount++;

                    //Updating Progress bar with Order Count
                    //===================================================================================================================================
                    _progressValue.Value = orderCount;
                    _progress?.Report(_progressValue);

                    //Check if Order is already imported and if it is set to overwrite
                    //===================================================================================================================================
                    OrderState orderState = (OrderState)a2pOrder.SalesDocumentState;
                 
                        _progressValue.ProgressTask1 = $"Importing Order# {a2pOrder.Order}.Deleting exiting data.";
                        _progress?.Report(_progressValue);

                    //Updating Progress bar with Order Number and progress
                    //===================================================================================================================================
                    _progressValue.ProgressTask1 = $"Importing Order# {a2pOrder.Order}. Order {orderCount}  of {a2pOrders.Count}";
                    _progress?.Report(_progressValue);

                    //Inserting Materials and Items
                    //===================================================================================================================================
                    await _writeItemService.DeleteAsync(a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion, order: a2pOrder.Order);
                    await _prefSuiteService.InsertItemsAsync(_progressValue, _progress);
                    await _writeItemService.InsertListAsync(_progressValue, _progress);
                    await _writeMaterialService.InsertListAsync(_progressValue, _progress);

                }
                orderCount++;
                _progressValue.Value = orderCount;
                _progress?.Report(_progressValue);
            }

            catch (Exception ex)
            {
                _logService.Error("Mapping handler service: Unhandled Error while mapping data. Exception: ${Exception} ", ex.Message);

            }

        }
    }
}
