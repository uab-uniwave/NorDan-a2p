using System.Data;

using a2p.Application.DTOs;
using a2p.Application.Interfaces.Excel;
using a2p.Application.Interfaces.Files;
using a2p.Application.Interfaces.Orchestrators;
using a2p.Application.Interfaces.PrefSuite;
using a2p.Application.Models;
using a2p.Domain.Enums;
using a2p.Infrastructure.Services.FileServices;

using Microsoft.Extensions.Logging;

namespace a2p.Infrastructure.Services.Orchestartors
{
    public class ReadService : IReadService
    {
        private readonly ILogger<ReadService> _logger;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        private readonly IPrefSuiteDataService _prefSuiteDataService;
        private readonly IExcelParserSchuco _excelParserSchuco;
        private readonly IExcelParserTechDesign _excelParserTechDesign;
        private List<OrderDto> _ordersDto;

        private ProgressValue _progressValue;
        private IProgress<ProgressValue> _progress;
        public ReadService(ILogger<ReadService> logger,
                           IFileService fileService,
                           IExcelService excelService,
                           IPrefSuiteDataService prefSuiteDataService,
                            IExcelParserTechDesign excelparserTechesign,
                           IExcelParserSchuco excelParserSchuco
                   )
        {

            _logger = logger;
            _fileService = fileService;
            _excelService = excelService;
            _excelParserTechDesign = excelparserTechesign;
            _excelParserSchuco = excelParserSchuco;
            _prefSuiteDataService = prefSuiteDataService;
            _ordersDto = [];
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }

        public async Task<List<OrderDto>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _ordersDto = [];
            _progressValue = progressValue;
            _progress = progress ?? new Progress<ProgressValue>();

            try
            {
                //==================================================================================================================================
                //🔵 Get FilesDto
                //==================================================================================================================================
                List<string> files = _fileService.GetFiles();
                if (files.Count == 0)
                {

                    return [];

                }

                //==================================================================================================================================
                //🔵 Get Orders list
                //==================================================================================================================================
                List<string> orders = GetOrders(files);

                //==================================================================================================================================
                //🔵 Create OrdersDto List   
                //==================================================================================================================================

                for (int i = 0; i < orders.Count; i++)
                {
                    List<string> orderFiles = GetOrderFiles(orders[i], files);
                    OrderDto orderDto = new();

                    orderDto.OrderNumber = orders[i];
                    orderDto.ItemsDto = [];
                    orderDto.MaterialsDto = [];
                    orderDto.ErrorsDto = [];
                    orderDto.Currency = "Unknown";
                    orderDto.ExchangeRate = 1;

                    for (int j = 0; j < orderFiles.Count; j++)
                    {
                        FileDto fileDto = new();
                        fileDto.OrderNumber = orders[i];
                        fileDto.FilePath = orderFiles[j];
                        fileDto.FileName = Path.GetFileName(orderFiles[j]);
                        fileDto.IsLocked = _fileService.IsLocked(orderFiles[j]);
                        orderDto.FilesDto.Add(fileDto);

                    }

                    if (orderDto.FilesDto.Any())
                    {

                        orderDto.FileCount = orderDto.FilesDto.Count;
                        orderDto.FileList = string.Join(", ", orderDto.FilesDto.Select(f => f.FileName));

                        if (orderDto.FilesDto.Any(f => f.IsLocked))
                        {
                            orderDto.LockedCount = orderDto.FilesDto.Count(f => f.IsLocked);
                            orderDto.LockedFiles = string.Join(", ", orderDto.FilesDto.Where(f => f.IsLocked).Select(f => f.FileName));
                        }

                        orderDto.SourceAppType = orderDto.FilesDto.Any(f => !string.IsNullOrEmpty(f.FileName) && f.FileName.Contains("calculation", System.StringComparison.OrdinalIgnoreCase))
                            ? SourceAppType.Schuco
                            : orderDto.FilesDto.Any(f => !string.IsNullOrEmpty(f.FileName) && f.FileName.Contains("Price_Details", System.StringComparison.OrdinalIgnoreCase))
                                ? SourceAppType.TechDesign
                                : SourceAppType.Unknown;

                    }
                    _ordersDto.Add(orderDto);
                }

                //==================================================================================================================================
                //🔵 Initialize Progress Bar
                //==================================================================================================================================
                _progressValue.Value = 0;

                _progressValue.MaxValue = _ordersDto.Count * 5;
                _progressValue.ProgressTask1 = $"Found {_ordersDto.Count} orders!";
                _progress?.Report(_progressValue);

                //==================================================================================================================================
                //🔵 Get OrderNumber FilesDto Progress Bar 1
                //==================================================================================================================================

                for (int i = 0; i < _ordersDto.Count; i++)
                {
                    _progressValue.Value++;
                    _progressValue.ProgressTask1 = string.Empty;
                    _progressValue.ProgressTask2 = $"Searching Orders FilesDto {i + 1} of {_ordersDto.Count} - OrderNumber #{_ordersDto[i].OrderNumber}";
                    _progress?.Report(_progressValue);

                    _ordersDto[i] = await GetOrderSalesDocumentAsync(_ordersDto[i]);
                    _ordersDto[i] = await GetOrderSalesDocumentState(_ordersDto[i]);
                    _ordersDto[i] = await GetOrderWorksheetsAsync(_ordersDto[i]);
                    _ordersDto[i] = await SetSalesDocumentReadErrors(_ordersDto[i]);

                    for (int j = 0; j < _ordersDto[i].FilesDto.Count; j++)
                    {
                        _progressValue.Value++;
                        _progress?.Report(_progressValue);

                        for (int k = 0; k < _ordersDto[i].FilesDto[j].Worksheets.Count; k++)
                        {
                            _progressValue.ProgressTask2 = $"WorksheetDto #{_ordersDto[i].FilesDto[j].Worksheets[k].Name}";

                            SourceAppType sourceAppType = _ordersDto[i].SourceAppType;
                            WorksheetType worksheetType = _ordersDto[i].FilesDto[j].Worksheets[k].WorksheetType;

                            if (sourceAppType == SourceAppType.Unknown)
                            {
                                _logger.LogError("Excel files format not recognized. Source application is Unknown. FileDto {$Filename}."
                                , _ordersDto[i].FilesDto[j].FileName);
                                continue;
                            }

                            if (worksheetType == WorksheetType.Unknown)
                            {
                                _logger.LogError(" Excel files format not recognized. WorksheetDto type  is Unknown. FileDto {$Filename}.",
                                  _ordersDto[i].FilesDto[j].FileName);
                                _ordersDto[i].ErrorsDto.Add(new ErrorDto
                                {
                                    OrderNumber = _ordersDto[i].OrderNumber,
                                    Level = ErrorLevel.Fatal,
                                    Code = ErrorCode.Excel_Read_Workbook_Source_Application_Format_Unknown,
                                    Message = $"OrderNumber {_ordersDto[i].OrderNumber}. FileDto ${_ordersDto[i].FilesDto[j].FileName} contains worksheet with unknown type."
                                });
                                continue;
                            }

                            //=====================================================================================================``
                            //🔵 TechnoDesign Items
                            //=====================================================================================================
                            if (sourceAppType == SourceAppType.TechDesign && worksheetType == WorksheetType.Items)
                            {
                                List<ItemDto> result = await _excelParserTechDesign.MapItemsAsync(_ordersDto[i].FilesDto[j].Worksheets[k], _progressValue, _progress);
                                _ordersDto[i].ItemsDto.AddRange(result);
                            }

                            //=====================================================================================================``
                            //🔵 Schuco Items
                            //=================================`====================================================================
                            else if (sourceAppType == SourceAppType.Schuco && worksheetType == WorksheetType.Items)
                            {
                                List<ItemDto> result = await _excelParserSchuco.MapItemsAsync(_ordersDto[i].FilesDto[j].Worksheets[k], _progressValue, _progress);
                                _ordersDto[i].ItemsDto.AddRange(result);
                            }

                            //=====================================================================================================``
                            //🔵 TechnoDesign Materals
                            //=====================================================================================================
                            else if (sourceAppType == SourceAppType.TechDesign && worksheetType == WorksheetType.Materials)
                            {
                                List<MaterialDto> result = await _excelParserTechDesign.MapMaterialsAsync(_ordersDto[i].FilesDto[j].Worksheets[k], _progressValue, _progress);
                                _ordersDto[i].MaterialsDto.AddRange(result);
                            }
                            //=====================================================================================================``
                            //🔵 TechnoDesign Materals
                            //=====================================================================================================
                            else if (sourceAppType == SourceAppType.Schuco && worksheetType == WorksheetType.Materials)
                            {
                                List<ItemDto> result = await _excelParserSchuco.MapItemsAsync(_ordersDto[i].FilesDto[j].Worksheets[k], _progressValue, _progress);
                                _ordersDto[i].ItemsDto.AddRange(result);
                            }

                            //=======================================================================================
                            //🔵 Materials WorksheetDto
                            //=======================================================================================
                            else
                            {

                                if (_ordersDto[i].SourceAppType == SourceAppType.Unknown)
                                {
                                    continue;
                                }
                                else if (_ordersDto[i].SourceAppType == SourceAppType.TechDesign)
                                {
                                    List<MaterialDto> result = await _excelParserTechDesign.MapMaterialsAsync(_ordersDto[i].FilesDto[j].Worksheets[k], _progressValue, _progress);
                                    _ordersDto[i].MaterialsDto.AddRange(result);

                                }
                                else if (_ordersDto[i].SourceAppType == SourceAppType.Schuco)
                                {
                                    List<MaterialDto> result = await _excelParserSchuco.MapMaterialsAsync(_ordersDto[i].FilesDto[j].Worksheets[k], _progressValue, _progress);
                                    _ordersDto[i].MaterialsDto.AddRange(result);
                                }
                                else
                                {
                                    _logger.LogError("Excel files format not recognized. Source application is Unknown. FileDto {$Filename}.", _ordersDto[i].FilesDto[j].FileName);
                                    continue;
                                }

                            }

                        }
                    }
                }
                return await Task.Run(() => _ordersDto);
            }
            catch (Exception ex)
            {

                _logger.LogError("PrefSuite Service: Unhandled error reading orders. Exception {$Exception}", ex.Message);
                return _ordersDto;
            }

        }

        private List<string> GetOrders(List<string> files)
        {
            try
            {
                List<string> orders = files
                    .Select(f => Path.GetFileName(f).Split(' ', '_')[0]) // Assuming order number is before the first underscore
                    .Distinct()
                    .ToList() ?? [];
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError("{$Class}.{$Method}. Unhandled error getting orders! Exception: {$Exception}",
                    nameof(FileService),
                    nameof(GetOrders),
                    ex.Message);
                return [];
            }
        }
        private List<string> GetOrderFiles(string order, List<string> files)
        {
            try
            {

                List<string> orderFiles = files.Where(f => f.Contains(order) && !f.Contains("~$") && f.EndsWith(".xlsx")).ToList(); // Get all files that match the order number

                return orderFiles ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError("{$Class}.{$Method}. Unhandled error getting ordr files! Exception: {$Exception}",
                    nameof(FileService),
                    nameof(GetOrderFiles),
                    ex.Message);
                return [];
            }
        }
        private async Task<OrderDto> GetOrderWorksheetsAsync(OrderDto order)
        {
            try
            {
                for (int i = 0; i < order.FilesDto.Count; i++)
                {

                    List<WorksheetDto> worksheets = await _excelService.GetWorksheetsAsync(order.FilesDto[i], _progressValue, _progress);

                    if (worksheets == null)
                    {
                        continue;
                    }
                    order.FilesDto[i].Worksheets.AddRange(worksheets);
                }

                return order;

            }
            catch (Exception ex)
            {
                _logger.LogError($"ErrorDto in GetOrderWorksheets: {ex.Message}");
                return order;
            }
        }
        private async Task<OrderDto> GetOrderSalesDocumentAsync(OrderDto order)
        {
            try
            {

                (int?, int?) result = await _prefSuiteDataService.GetSalesDocumentAsync(order.OrderNumber);
                if (result.Item1 < 1 || result.Item2 < 1)
                {

                    order.ErrorsDto.Add(new ErrorDto
                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Fatal,
                        Code = ErrorCode.Business_Process_Order_Not_Found_In_PreSuite,
                        Message = $"OrderNumber# {order.OrderNumber} not exists in PrefSuite DB !"
                    }
                    );
                }

                order.SalesDocumentDto.Number = result.Item1 ?? -1;
                order.SalesDocumentDto.Version = result.Item2 ?? -1;
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    " \n{$Exception}",
               nameof(ReadService),
                    nameof(GetOrderSalesDocumentAsync),
                    order.OrderNumber ?? string.Empty,
                    ex.Message);
                return order;
            }
        }
        private async Task<OrderDto> GetOrderSalesDocumentState(OrderDto order)
        {
            try
            {

                int state = await _prefSuiteDataService.GetSalesDocumentStateAsync(order.SalesDocumentDto.Number, order.SalesDocumentDto.Version);
                order.SalesDocumentDto.State = (OrderState)state;
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    " \n{$Exception}",
               nameof(ReadService),
                    nameof(GetOrderSalesDocumentAsync),
                    order.OrderNumber ?? string.Empty,
                    ex.Message);

                return order;
            }

        }
        private async Task<OrderDto> SetSalesDocumentReadErrors(OrderDto order)
        {
            try
            {

                if (order.SalesDocumentDto.State.HasFlag(OrderState.PurchaseOrdersExist))
                {
                    order.ErrorsDto.Add(new ErrorDto

                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Fatal,
                        Code = ErrorCode.Business_Process_Order_Purcahes_Has_Been_Done,
                        Message = $"OrderNumber {order.OrderNumber} - {order.SalesDocumentDto.Number}/{order.SalesDocumentDto.Version} contains purchase orders.\nPlease remove purchase orders and try load data again."

                    });
                    return await Task.Run(() => order);

                }

                if (order.SalesDocumentDto.State.HasFlag(OrderState.MaterialNeedsInserted))
                {
                    order.ErrorsDto.Add(new ErrorDto
                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.Business_Process_Order_MaterialNeeds_Calculated,
                        Message = $"OrderNumber {order.OrderNumber} - {order.SalesDocumentDto.Number}/{order.SalesDocumentDto.Version}  contains calculated material needs!"

                    });
                    return await Task.Run(() => order);

                }

                if (order.SalesDocumentDto.State.HasFlag(OrderState.ItemsCreated))
                {
                    order.ErrorsDto.Add(new ErrorDto
                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.Business_Process_Order_Already_Contains_Items,
                        Message = $"OrderNumber {order.OrderNumber} - {order.SalesDocumentDto.Number}/{order.SalesDocumentDto.Version}  contains PrefSuite items!"

                    });
                    return await Task.Run(() => order);

                }

                if (order.SalesDocumentDto.State.HasFlag(OrderState.A2PItemsImported))
                {
                    order.ErrorsDto.Add(new ErrorDto
                    {
                        OrderNumber = order.OrderNumber,
                        Level = ErrorLevel.Warning,
                        Code = ErrorCode.Business_Process_Order_Already_Contains_Items,
                        Message = $"OrderNumber {order.OrderNumber} - {order.SalesDocumentDto.Number}/{order.SalesDocumentDto.Version}  contains imported items worksheet!"

                    });
                    return await Task.Run(() => order);

                }
                return await Task.Run(() => order);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                _logger.LogError("Unhandled error {$Class}.{Method}." +
                    "\nOrder {$OrderNumber}." +
                    " \n{$Exception}",
               nameof(ReadService),
                    nameof(GetOrderSalesDocumentAsync),
                    order.OrderNumber ?? string.Empty,
                    ex.Message);

                return await Task.Run(() => order);
            }

        }

    }
}
