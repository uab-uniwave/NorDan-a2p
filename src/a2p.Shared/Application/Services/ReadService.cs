// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;

using a2p.Shared.Application.Interfaces;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Application.Services
{
    public class ReadService : IReadService
    {
        private readonly ILogService _logService;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelReadService;
        private readonly ISQLRepository _sqlRepository;
        private readonly IMapperSapaV2 _mapperSapaV2;
        private readonly IMapperSapaV2 _mapperSapaV1;
        private readonly IMapperSapaV2 _mapperSchuco;
        private List<A2POrder> _a2pOrders;

        private ProgressValue _progressValue;
        private IProgress<ProgressValue> _progress;
        public ReadService(ILogService logService,
                           IFileService fileService,
                           IExcelService excelReadService,
                           ISQLRepository sqlRepository,
                            IMapperSapaV2 mapperSapaV2,
                            IMapperSapaV2 mapperSapaV1,
                            IMapperSapaV2 mapperSchuco
                   )

        {

            _logService = logService;
            _fileService = fileService;
            _excelReadService = excelReadService;
            _sqlRepository = sqlRepository;
            _mapperSapaV2 = mapperSapaV2;
            _mapperSapaV1 = mapperSapaV1;
            _mapperSchuco = mapperSchuco;
            _a2pOrders = [];
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }
        public async Task<List<A2POrder>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _a2pOrders = [];
            _progressValue = progressValue;
            _progress = progress ?? new Progress<ProgressValue>();

            try
            {
                //==================================================================================================================================
                //🔵 Get Files
                //==================================================================================================================================
                List<string>? allFiles = _fileService.GetFiles();
                if (allFiles == null || allFiles.Count == 0)
                {

                    return _a2pOrders;

                }

                //==================================================================================================================================
                //🔵 Get Files
                //==================================================================================================================================
                _a2pOrders = await GetOrders(allFiles);
                try
                {
                    if (_a2pOrders == null || _a2pOrders.Count == 0)
                    {
                        return _a2pOrders ?? [];
                    }

                    _progressValue.MaxValue = _a2pOrders.Count * 5;
                    _progressValue.ProgressTask1 = $"Found {_a2pOrders.Count} orders!";
                    _progress?.Report(_progressValue);

                }
                catch (Exception ex)
                {
                    _logService.Error("Unhandled error {$Class}.{Method}." +
                        " \n{$Exception}",
                   nameof(ReadService),
                        nameof(GetOrderSalesDocumentState),
                       ex.Message);
                }
                //==================================================================================================================================
                //🔵 Get Order Files Progress Bar 1
                //==================================================================================================================================

                for (int i = 0; i < _a2pOrders.Count; i++)
                {
                    _progressValue.Value++;
                    _progressValue.ProgressTask1 = string.Empty;
                    _progressValue.ProgressTask2 = $"Searching Orders Files {i + 1} of {_a2pOrders.Count} - Order #{_a2pOrders[i].Order}";
                    _progress?.Report(_progressValue);
                    try
                    {

                        _a2pOrders[i] = await GetOrderFilesAsync(_a2pOrders[i]);
                        _a2pOrders[i] = await GetOrderSalesDocumentAsync(_a2pOrders[i]);
                        _a2pOrders[i] = await GetOrderSalesDocumentState(_a2pOrders[i]);
                        _a2pOrders[i] = await GetOrderWorksheetsAsync(_a2pOrders[i]);
                        for (int j = 0; j < _a2pOrders[i].Files.Count; j++)
                        {
                            _progressValue.Value++;
                            _progress?.Report(_progressValue);

                            for (int k = 0; k < _a2pOrders[i].Files[j].Worksheets.Count; k++)
                            {
                                _progressValue.ProgressTask2 = $"Worksheet #{_a2pOrders[i].Files[j].Worksheets[k].Name}";

                                WorksheetType type = _a2pOrders[i].Files[j].Worksheets[k].WorksheetType;

                                //=======================================================================================
                                //🔵 Unknown Worksheet
                                //=======================================================================================
                                if (type == WorksheetType.Unknown)
                                {
                                    continue;
                                }
                                //=======================================================================================
                                //🔵 Items Worksheet
                                //=======================================================================================
                                if (type == WorksheetType.Items)
                                {
                                    //🔵 Unknown Items
                                    //=======================================================================================
                                    if (_a2pOrders[i].SourceAppType == SourceAppType.Unknown)
                                    {
                                        _logService.Error("{$Class}.{$Method}." +
                                            "\nUnknown source of file (SapaV1?, SapaV2, Schuco).Order {$Order}.",
                                            nameof(ReadService),
                                            nameof(GetOrderSalesDocumentState),
                                            _a2pOrders[i].Order ?? string.Empty);
                                        continue;
                                    }

                                    //🔵 Sapa V2 Items
                                    //=====================================================================================================
                                    else if (_a2pOrders[i].SourceAppType == SourceAppType.SapaV2)
                                    {
                                        (List<DTO.ItemDTO>, List<A2PError>) result = await _mapperSapaV2.MapItemsAsync(_a2pOrders[i].Files[j].Worksheets[k], _progressValue, _progress);

                                        if (result.Item1 != null && result.Item1.Count > 0)
                                        {
                                            _a2pOrders[i].Items.AddRange(result.Item1);

                                        }
                                        if (result.Item2 != null && result.Item2.Count > 0)
                                        {
                                            _a2pOrders[i].ErrorsRead.AddRange(result.Item2);
                                        }

                                    }

                                    //🔵 Sapa V1 Items
                                    //=====================================================================================================
                                    else if (_a2pOrders[i].SourceAppType == SourceAppType.SapaV1)
                                    {
                                        throw new NotImplementedException("Sapa V1 Items not implemented yet.");
                                    }

                                    //🔵 Schuco Items
                                    //=====================================================================================================
                                    else
                                    {
                                        throw new NotImplementedException("Sapa V1 Items not implemented yet.");
                                    }
                                }

                                //=======================================================================================
                                //🔵 Materials Worksheet
                                //=======================================================================================
                                else
                                {
                                    //🔵 Unknown Materials
                                    //=======================================================================================
                                    if (_a2pOrders[i].SourceAppType == SourceAppType.Unknown)
                                    {
                                        continue;
                                    }

                                    //🔵 Sapa V2 Materials
                                    //=======================================================================================
                                    else if (_a2pOrders[i].SourceAppType == SourceAppType.SapaV2)
                                    {

                                        (List<DTO.MaterialDTO>, List<A2PError>) result = await _mapperSapaV2.MapMaterialsAsync(_a2pOrders[i].Files[j].Worksheets[k], _progressValue, _progress);

                                        if (result.Item1 != null && result.Item1.Count > 0)
                                        {
                                            _a2pOrders[i].Materials.AddRange(result.Item1);
                                        }
                                        if (result.Item2 != null && result.Item2.Count > 0)
                                        {
                                            _a2pOrders[i].ErrorsRead.AddRange(result.Item2);
                                        }

                                    }

                                    //🔵 Sapa V1 Materials
                                    //=======================================================================================
                                    else if (_a2pOrders[i].SourceAppType == SourceAppType.SapaV1)
                                    {
                                        throw new NotImplementedException("Sapa V1 Items not implemented yet.");

                                    }

                                    //🔵 Schuco Materials
                                    //=======================================================================================
                                    else
                                    {

                                        throw new NotImplementedException("Sapa V1 Items not implemented yet.");
                                    }

                                }
                            }
                        }
                        _a2pOrders[i] = SetSalesDocumentReadErrors(_a2pOrders[i]);

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Unhandled error {$Class}.{Method}." +
                            "\nOrder {$Order}." +
                            " \n{$Exception}",
                       nameof(ReadService),
                            nameof(GetOrderSalesDocumentState),
                            _a2pOrders[i].Order ?? string.Empty,
                            ex.Message);
                        continue;
                    }
                }
                return _a2pOrders;
            }
            catch (Exception ex)
            {
                _logService.Error("PrefSuite Service: Unhandled error reading orders. Exception {$Exception}", ex.Message);
                return _a2pOrders;
            }

        }

        private async Task<List<A2POrder>> GetOrders(List<string>? files)
        {

            if (files == null || files.Count == 0)
            {
                return await Task.Run(() => _a2pOrders);
            }

            try
            {
                List<string> workingFiles = files.Select(f => f)
                                      .Where(f => !f.Contains("~$") && f.EndsWith(".xlsx")).ToList();

                if (workingFiles == null || workingFiles.Count == 0)
                {
                    return await Task.Run(() => _a2pOrders);
                }

                List<string> orderNumbers = workingFiles.Select(f => Path.GetFileName(f)!.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0])
                      .Distinct()
                      .OrderBy(f => f)
                      .ToList();

                foreach (string orderNumber in orderNumbers)
                {

                    A2POrder a2pOrder = new()
                    {
                        Order = orderNumber,
                        Items = [],
                        Materials = [],
                        ErrorsRead = [],
                        ErrorsWrite = [],
                        SalesDocumentNumber = -1,
                        SalesDocumentVersion = -1,
                        SalesDocumentState = -1,
                        Currency = "Unknown",
                        ExchangeRate = 1

                    };

                    _a2pOrders.Add(a2pOrder);

                }

                return await Task.Run(() => _a2pOrders);

            }

            catch (Exception ex)
            {
                _logService.Verbose(
           "{$Class}.{$Method}. Unhandled error getting orders. Exception: {Exception}.",
           nameof(IReadService),
           nameof(GetOrders),
            ex.Message
          );

                return await Task.Run(() => _a2pOrders);

            }
        }

        private async Task<A2POrder> GetOrderFilesAsync(A2POrder a2pOrder)
        {

            try
            {
                await Task.Run(() =>
                {

                    List<string> files = _fileService.GetFiles()!
                       .Select(f => f)
                       .Where(f => f
                       .EndsWith(".xlsx") && !f
                       .Contains("~$") && f
                       .Contains(a2pOrder.Order)).ToList();

                    for (int i = 0; i < files.Count; i++)
                    {

                        A2PFile a2pFile = new()
                        {
                            File = files[i],
                            Order = a2pOrder.Order,
                            FileName = System.IO.Path.GetFileName(files[i]),
                            IsLocked = _fileService.IsLocked(files[i]),
                            FilePath = System.IO.Path.GetFullPath(files[i]),
                            Worksheets = []
                        };

                        if (a2pFile.IsLocked)
                        {

                            a2pOrder.ErrorsRead.Add(new A2PError
                            {
                                Order = a2pOrder.Order,
                                Level = ErrorLevel.Fatal,
                                Code = ErrorCode.FileSystemRead,
                                Message = $"Order {a2pOrder.Order}. File ${a2pFile.FileName} is Locked."

                            });
                        }
                        a2pOrder.Files.Add(a2pFile);
                    }

                });
                return a2pOrder;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error in GetOrderFiles: {ex.Message}");
                return a2pOrder;
            }
        }

        private async Task<A2POrder> GetOrderWorksheetsAsync(A2POrder a2pOrder)

        {
            try
            {
                for (int i = 0; i < a2pOrder.Files.Count; i++)
                {

                    List<A2PWorksheet> worksheets = await _excelReadService.GetWorksheetsAsync(a2pOrder.Files[i], _progressValue, _progress);

                    if (worksheets == null)
                    {
                        continue;
                    }
                    a2pOrder.Files[i].Worksheets.AddRange(worksheets);
                }

                return a2pOrder;

            }
            catch (Exception ex)
            {
                _logService.Error($"Error in GetOrderWorksheets: {ex.Message}");
                return a2pOrder;
            }
        }

        private async Task<A2POrder> GetOrderSalesDocumentAsync(A2POrder a2pOrder)
        {

            (int, int) salesDocument = (-1, -1);
            try
            {

                (int, int) result = await _sqlRepository.GetSalesDocumentAsync(a2pOrder.Order);
                if (result.Item1 < 1 || result.Item2 < 1)
                {
                    salesDocument.Item1 = -1;
                    salesDocument.Item2 = -1;

                    a2pOrder.ErrorsRead.Add(new A2PError
                    {
                        Order = a2pOrder.Order,
                        Level = ErrorLevel.Fatal,
                        Code = ErrorCode.DatabaseRead_Order,
                        Message = $"Order# {a2pOrder.Order} not exists in PrefSuite DB !"
                    }
                    );
                }
                else
                {
                    a2pOrder.SalesDocumentNumber = result.Item1;
                    a2pOrder.SalesDocumentVersion = result.Item2;
                }

                return a2pOrder;
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    " \n{$Exception}",
               nameof(ReadService),
                    nameof(GetOrderSalesDocumentAsync),
                    a2pOrder.Order ?? string.Empty,
                    ex.Message);

                return a2pOrder;
            }
        }
        private async Task<A2POrder> GetOrderSalesDocumentState(A2POrder a2pOrder)
        {
            try
            {

                a2pOrder.SalesDocumentState = await _sqlRepository.GetSalesDocumentStateAsync(a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);

                return a2pOrder;
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    " \n{$Exception}",
               nameof(ReadService),
                    nameof(GetOrderSalesDocumentAsync),
                    a2pOrder.Order ?? string.Empty,
                    ex.Message);

                return a2pOrder;
            }

        }
        private A2POrder SetSalesDocumentReadErrors(A2POrder a2pOrder)
        {
            try
            {

                OrderState orderState = (OrderState) a2pOrder.SalesDocumentState;

                if (orderState.HasFlag(OrderState.PurchaseOrdersExist))
                {
                    a2pOrder.ErrorsRead.Add(new A2PError
                    {
                        Order = a2pOrder.Order,
                        Level = ErrorLevel.Fatal,
                        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                        Message = $"Order {a2pOrder.Order} - {a2pOrder.SalesDocumentNumber}/{a2pOrder.SalesDocumentVersion} contains purchase orders.\nPlease remove purchase orders and try load data again."

                    });
                    return a2pOrder;

                }

                if (orderState.HasFlag(OrderState.MaterialNeedsInserted))
                {
                    a2pOrder.ErrorsRead.Add(new A2PError
                    {
                        Order = a2pOrder.Order,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                        Message = $"Order {a2pOrder.Order} - {a2pOrder.SalesDocumentNumber}/{a2pOrder.SalesDocumentVersion} contains calculated material needs!"

                    });
                    return a2pOrder;

                }

                if (orderState.HasFlag(OrderState.ItemsCreated))
                {
                    a2pOrder.ErrorsRead.Add(new A2PError
                    {
                        Order = a2pOrder.Order,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                        Message = $"Order {a2pOrder.Order} - {a2pOrder.SalesDocumentNumber}/{a2pOrder.SalesDocumentVersion} contains PrefSuite items!"

                    });
                    return a2pOrder;

                }

                if (orderState.HasFlag(OrderState.A2PItemsImported))
                {
                    a2pOrder.ErrorsRead.Add(new A2PError
                    {
                        Order = a2pOrder.Order,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                        Message = $"Order {a2pOrder.Order} - {a2pOrder.SalesDocumentNumber}/{a2pOrder.SalesDocumentVersion} contains imported items worksheet!"

                    });

                }

                if (orderState.HasFlag(OrderState.A2PMaterialsImported))
                {
                    a2pOrder.ErrorsRead.Add(new A2PError
                    {
                        Order = a2pOrder.Order,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                        Message = $"Order {a2pOrder.Order} - {a2pOrder.SalesDocumentNumber}/{a2pOrder.SalesDocumentVersion} contains imported material worksheets!"

                    });

                }

                return a2pOrder;

            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$Order}." +
                    " \n{$Exception}",
               nameof(ReadService),
                    nameof(GetOrderSalesDocumentAsync),
                    a2pOrder.Order ?? string.Empty,
                    ex.Message);

                return a2pOrder;
            }

        }

    }
}
