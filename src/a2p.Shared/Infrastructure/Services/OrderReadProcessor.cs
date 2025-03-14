// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Infrastructure.Services
{
    public class OrderReadProcessor : IOrderReadProcessor
    {
        private readonly ILogService _logService;
        private readonly IFileService _fileService;
        private readonly IExcelReadService _excelReadService;
        private readonly IPrefSuiteService _prefSuiteService;
        private readonly DataCache _dataCache;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue> _progress;
        public OrderReadProcessor(ILogService logService,
                           IFileService fileService,
                           IExcelReadService excelReadService,
                           DataCache dataCache, IPrefSuiteService prefSuiteService, IExcelReadService excelService)

        {
            _logService = logService;
            _fileService = fileService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _dataCache = dataCache;
            _prefSuiteService = prefSuiteService;
            _excelReadService = excelService;
        }

        public async Task ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            _progressValue = progressValue;
            _progress = progress ?? new Progress<ProgressValue>();

            try
            {

                _progressValue = progressValue;
                _progress = progress ?? new Progress<ProgressValue>();

                _progressValue.ProgressTitle = "Loading Orders Files... ";
                _progressValue.ProgressTask1 = $"Reading Orders ...";

                await _fileService.GetOrdersAsync(_progressValue, _progress);

                await _prefSuiteService.GetSalesDocumentStates(_progressValue, _progress);

                List<A2POrder> a2pOrders = _dataCache.GetAllOrders();


            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error reading a2pOrders");

            }

        }

    }
}
