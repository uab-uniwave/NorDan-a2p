using a2p.Application.Interfaces;
using a2p.Application.Interfaces.MappingService;
using a2p.Domain.Entities;
using a2p.Domain.Enums;
using a2p.Domain.Interfaces;
using a2p.Domain.Models;

using System.Data;


namespace a2p.Infrastructure.Services
{
    public class ReadService : IReadService
    {
        private readonly ILogService _logService;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelReaderService;
        private readonly IMyRepository _orderRepository;
        private readonly IPrefSuiteDataService _prefSuiteDataService;
        private readonly IMapperTechDesign _mapperSapaTechDesign;
        private readonly IMapperSapa _mapperSapa;
        private readonly IMapperSchuco _mapperSchuco;
        private List<OrderEntity> _orders;

        private ProgressValue _progressValue;
        private IProgress<ProgressValue> _progress;
        public ReadService(ILogService logService,
                           IFileService fileService,
                           IExcelService excelReadService,
                           IMyRepository orderRepository,
                            IMapperTechDesign mapperTechDesign,
                            IMapperSapa mapperSapa,
                            IMapperSchuco mapperSchuco, IPrefSuiteDataService prefSuiteDataService
                   )

        {

            _logService = logService;
            _fileService = fileService;
            _excelReaderService = excelReadService;
            _orderRepository = orderRepository;
            _mapperSapaTechDesign = mapperTechDesign;
            _mapperSapa = mapperSapa;
            _mapperSchuco = mapperSchuco;
            _prefSuiteDataService = prefSuiteDataService;
            _orders = [];
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }
        public async Task<List<OrderEntity>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _orders = [];
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

                    return _orders;

                }

                //==================================================================================================================================
                //🔵 Get Files
                //==================================================================================================================================
                _orders = await GetOrders(allFiles);
                try
                {
                    if (_orders == null || _orders.Count == 0)
                    {
                        return _orders ?? [];
                    }

                    _progressValue.MaxValue = _orders.Count * 5;
                    _progressValue.ProgressTask1 = $"Found {_orders.Count} orders!";
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
                //🔵 Get OrderNumber Files Progress Bar 1
                //==================================================================================================================================

                for (int i = 0; i < _orders.Count; i++)
                {
                    _progressValue.Value++;
                    _progressValue.ProgressTask1 = string.Empty;
                    _progressValue.ProgressTask2 = $"Searching Orders Files {i + 1} of {_orders.Count} - OrderNumber #{_orders[i].OrderNumber}";
                    _progress?.Report(_progressValue);
                    try
                    {

                        _orders[i] = await GetOrderFilesAsync(_orders[i]);
                        _orders[i] = await GetOrderSalesDocumentAsync(_orders[i]);
                        _orders[i] = await GetOrderSalesDocumentState(_orders[i]);
                        _orders[i] = await GetOrderWorksheetsAsync(_orders[i]);
                        for (int j = 0; j < _orders[i].Files.Count; j++)
                        {
                            _progressValue.Value++;
                            _progress?.Report(_progressValue);

                            for (int k = 0; k < _orders[i].Files[j].Worksheets.Count; k++)
                            {
                                _progressValue.ProgressTask2 = $"Worksheet #{_orders[i].Files[j].Worksheets[k].Name}";

                                WorksheetType type = _orders[i].Files[j].Worksheets[k].WorksheetType;

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
                                    if (_orders[i].SourceAppType == SourceAppType.Unknown)
                                    {
                                        _logService.Error("{$Class}.{$Method}." +
                                            "\nUnknown source of file (Sapa, TechnoDesign, Schuco).OrderNumber {$OrderNumber}.",
                                            nameof(ReadService),
                                            nameof(GetOrderSalesDocumentState),
                                            _orders[i].OrderNumber ?? string.Empty);
                                        continue;
                                    }

                                    //🔵 TechnoDesign Items
                                    //=====================================================================================================
                                    else if (_orders[i].SourceAppType == SourceAppType.TechDesign)
                                    {
                                        (List<ItemEntity>, List<ErrorEntity>) result = await _mapperSapaTechDesign.MapItemsAsync(_orders[i].Files[j].Worksheets[k], _progressValue, _progress);

                                        if (result.Item1 != null && result.Item1.Count > 0)
                                        {
                                            _orders[i].Items.AddRange(result.Item1);

                                        }
                                        if (result.Item2 != null && result.Item2.Count > 0)
                                        {
                                            _orders[i].Errors.AddRange(result.Item2);
                                        }

                                    }

                                    //🔵 Sapa Items
                                    //=====================================================================================================
                                    else if (_orders[i].SourceAppType == SourceAppType.Sapa)
                                    {
                                        throw new NotImplementedException("Sapa Items not implemented yet.");
                                    }

                                    //🔵 Schuco Items
                                    //=====================================================================================================
                                    else
                                    {
                                        throw new NotImplementedException("Schuco Items not implemented yet.");
                                    }
                                }

                                //=======================================================================================
                                //🔵 Materials Worksheet
                                //=======================================================================================
                                else
                                {
                                    //🔵 Unknown Materials
                                    //=======================================================================================
                                    if (_orders[i].SourceAppType == SourceAppType.Unknown)
                                    {
                                        continue;
                                    }

                                    //🔵TechnoDesign Materials
                                    //=======================================================================================
                                    else if (_orders[i].SourceAppType == SourceAppType.TechDesign)
                                    {

                                        (List<MaterialEntity>, List<ErrorEntity>) result = await _mapperSapaTechDesign.MapMaterialsAsync(_orders[i].Files[j].Worksheets[k], _progressValue, _progress);

                                        if (result.Item1 != null && result.Item1.Count > 0)
                                        {
                                            _orders[i].Materials.AddRange(result.Item1);
                                        }
                                        if (result.Item2 != null && result.Item2.Count > 0)
                                        {
                                            _orders[i].Errors.AddRange(result.Item2);
                                        }

                                    }

                                    //🔵 Sapa Materials
                                    //=======================================================================================
                                    else if (_orders[i].SourceAppType == SourceAppType.Sapa)
                                    {
                                        throw new NotImplementedException("Sapa Items not implemented yet.");

                                    }

                                    //🔵 Schuco Materials
                                    //=======================================================================================
                                    else
                                    {

                                        throw new NotImplementedException("Schuco Items not implemented yet.");
                                    }

                                }
                            }
                        }
                        _orders[i] = SetSalesDocumentReadErrors(_orders[i]);

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Unhandled error {$Class}.{Method}." +
                            "\nOrder {$OrderNumber}." +
                            " \n{$Exception}",
                       nameof(ReadService),
                            nameof(GetOrderSalesDocumentState),
                            _orders[i].OrderNumber ?? string.Empty,
                            ex.Message);
                        continue;
                    }
                }
                return _orders;
            }
            catch (Exception ex)
            {
                _logService.Error("PrefSuite Service: Unhandled error reading orders. Exception {$Exception}", ex.Message);
                return _orders;
            }

        }

        private async Task<List<OrderEntity>> GetOrders(List<string>? files)
        {

            if (files == null || files.Count == 0)
            {
                return await Task.Run(() => _orders);
            }

            try
            {
                List<string> workingFiles = files.Select(f => f)
                                      .Where(f => !f.Contains("~$") && f.EndsWith(".xlsx")).ToList();

                if (workingFiles == null || workingFiles.Count == 0)
                {
                    return await Task.Run(() => _orders);
                }

                List<string> orderNumbers = workingFiles.Select(f => Path.GetFileName(f)!.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0])
                      .Distinct()
                      .OrderBy(f => f)
                      .ToList();

                foreach (string orderNumber in orderNumbers)
                {

                    OrderEntity order = new()
                    {
                        OrderNumber = orderNumber,
                        Items = [],
                        Materials = [],
                        Errors = [],
                        SalesDocumentNumber = -1,
                        SalesDocumentVersion = -1,
                        SalesDocumentState = -1,
                        Currency = "Unknown",
                        ExchangeRate = 1

                    };

                    _orders.Add(order);

                }

                return await Task.Run(() => _orders);

            }

            catch (Exception ex)
            {
                _logService.Verbose(
           "{$Class}.{$Method}. Unhandled error getting orders. Exception: {Exception}.",
           nameof(IReadService),
           nameof(GetOrders),
            ex.Message
          );

                return await Task.Run(() => _orders);

            }
        }

        private async Task<OrderEntity> GetOrderFilesAsync(OrderEntity order)
        {

            try
            {
                await Task.Run((Action)(() =>
                {

                    List<string> files = _fileService.GetFiles()!
                       .Select(f => f)
                       .Where(f => f
                       .EndsWith(".xlsx") && !f
                       .Contains("~$") && f
                       .Contains(order.OrderNumber)).ToList();

                    for (int i = 0; i < files.Count; i++)
                    {

                        a2p.Domain.Models.File file = new()
                        {
                            FullName = files[i],
                            OrderNumber = order.OrderNumber,
                            FileName = System.IO.Path.GetFileName(files[i]),
                            IsLocked = _fileService.IsLocked(files[i]),
                            FilePath = System.IO.Path.GetFullPath(files[i]),
                            Worksheets = []
                        };

                        if (file.IsLocked)
                        {

                            order.Errors.Add(new ErrorEntity
                            {
                                OrderNumber = order.OrderNumber,
                                Level = ErrorLevel.Fatal,
                                Code = ErrorCode.FileSystemReadWrite,
                                Message = $"OrderNumber {order.OrderNumber}. File ${file.FileName} is Locked."

                            });
                        }
                        order.Files.Add(file);
                    }

                }));
                return order;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error in GetOrderFiles: {ex.Message}");
                return order;
            }
        }

        private async Task<OrderEntity> GetOrderWorksheetsAsync(OrderEntity order)

        {
            try
            {
                for (int i = 0; i < order.Files.Count; i++)
                {

                    List<Worksheet> worksheets = await _excelReaderService.GetWorksheetsAsync(order.Files[i], _progressValue, _progress);

                    if (worksheets == null)
                    {
                        continue;
                    }
                    order.Files[i].Worksheets.AddRange(worksheets);
                }

                return order;

            }
            catch (Exception ex)
            {
                _logService.Error($"Error in GetOrderWorksheets: {ex.Message}");
                return order;
            }
        }

        private async Task<OrderEntity> GetOrderSalesDocumentAsync(OrderEntity order)
        {

            (int, int) salesDocument = (-1, -1);
            try
            {

                (int, int) result = await _prefSuiteDataService.GetSalesDocumentAsync(order.OrderNumber);
                if (result.Item1 < 1 || result.Item2 < 1)
                {
                    salesDocument.Item1 = -1;
                    salesDocument.Item2 = -1;

                    order.Errors.Add(new ErrorEntity
                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Fatal,
                        Code = ErrorCode.DatabaseRead_OrderReferenceNotFound,
                        Message = $"OrderNumber# {order.OrderNumber} not exists in PrefSuite DB !"
                    }
                    );
                }
                else
                {
                    order.SalesDocumentNumber = result.Item1;
                    order.SalesDocumentVersion = result.Item2;
                }

                return order;
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    " \n{$Exception}",
               nameof(ReadService),
                    nameof(GetOrderSalesDocumentAsync),
                    order.OrderNumber ?? string.Empty,
                    ex.Message);

                return order;
            }
        }
        private async Task<OrderEntity> GetOrderSalesDocumentState(OrderEntity order)
        {
            try
            {

                order.SalesDocumentState = await _prefSuiteDataService.GetSalesDocumentStateAsync(order.SalesDocumentNumber, order.SalesDocumentVersion);

                return order;
            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    " \n{$Exception}",
               nameof(ReadService),
                    nameof(GetOrderSalesDocumentAsync),
                    order.OrderNumber ?? string.Empty,
                    ex.Message);

                return order;
            }

        }
        private OrderEntity SetSalesDocumentReadErrors(OrderEntity order)
        {
            try
            {

                OrderState orderState = (OrderState)order.SalesDocumentState;

                if (orderState.HasFlag(OrderState.PurchaseOrdersExist))
                {
                    order.Errors.Add(new ErrorEntity
                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Fatal,
                        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                        Message = $"OrderNumber {order.OrderNumber} - {order.SalesDocumentNumber}/{order.SalesDocumentVersion} contains purchase orders.\nPlease remove purchase orders and try load data again."

                    });
                    return order;

                }

                if (orderState.HasFlag(OrderState.MaterialNeedsInserted))
                {
                    order.Errors.Add(new ErrorEntity
                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                        Message = $"OrderNumber {order.OrderNumber} - {order.SalesDocumentNumber}/{order.SalesDocumentVersion} contains calculated material needs!"

                    });
                    return order;

                }

                if (orderState.HasFlag(OrderState.ItemsCreated))
                {
                    order.Errors.Add(new ErrorEntity
                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                        Message = $"OrderNumber {order.OrderNumber} - {order.SalesDocumentNumber}/{order.SalesDocumentVersion} contains PrefSuite items!"

                    });
                    return order;

                }

                if (orderState.HasFlag(OrderState.A2PItemsImported))
                {
                    order.Errors.Add(new ErrorEntity
                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                        Message = $"OrderNumber {order.OrderNumber} - {order.SalesDocumentNumber}/{order.SalesDocumentVersion} contains imported items worksheet!"

                    });

                }

                if (orderState.HasFlag(OrderState.A2PMaterialsImported))
                {
                    order.Errors.Add(new ErrorEntity
                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                        Message = $"OrderNumber {order.OrderNumber} - {order.SalesDocumentNumber}/{order.SalesDocumentVersion} contains imported material worksheets!"

                    });

                }

                return order;

            }
            catch (Exception ex)
            {
                _logService.Error("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    " \n{$Exception}",
               nameof(ReadService),
                    nameof(GetOrderSalesDocumentAsync),
                    order.OrderNumber ?? string.Empty,
                    ex.Message);

                return order;
            }

        }

    }
}
