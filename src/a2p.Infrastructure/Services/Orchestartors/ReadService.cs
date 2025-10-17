using System.Data;

using a2p.Application.DTOs;
using a2p.Application.Interfaces.Excel;
using a2p.Application.Interfaces.Files;
using a2p.Application.Interfaces.Orchestrators;
using a2p.Application.Interfaces.PrefSuite;
using a2p.Application.Models;
using a2p.Domain.Enums;
using a2p.Infrastructure.Models.BaseModels;

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
        private List<OrderDto> _excelOrdersDto;

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
            _excelOrdersDto = [];
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }

        public async Task<List<OrderDto>?> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _excelOrdersDto = [];
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

                    return null;

                }

                //==================================================================================================================================
                //🔵 Get Files
                //==================================================================================================================================
                _excelOrdersDto = await GetOrders(allFiles);
                if (_excelOrdersDto == null || _excelOrdersDto.Count == 0)
                {
                    return null;
                }

                _progressValue.MaxValue = _excelOrdersDto.Count * 5;
                _progressValue.ProgressTask1 = $"Found {_excelOrdersDto.Count} orders!";
                _progress?.Report(_progressValue);

                //==================================================================================================================================
                //🔵 Get OrderNumber Files Progress Bar 1
                //==================================================================================================================================

                for (int i = 0; i < _excelOrdersDto.Count; i++)
                {
                    _progressValue.Value++;
                    _progressValue.ProgressTask1 = string.Empty;
                    _progressValue.ProgressTask2 = $"Searching Orders Files {i + 1} of {_excelOrdersDto.Count} - OrderNumber #{_excelOrdersDto[i].OrderNumber}";
                    _progress?.Report(_progressValue);

                    _excelOrdersDto[i] = await GetOrderFilesAsync(_excelOrdersDto[i]);
                    _excelOrdersDto[i] = await GetOrderSalesDocumentAsync(_excelOrdersDto[i]);
                    _excelOrdersDto[i] = await GetOrderSalesDocumentState(_excelOrdersDto[i]);
                    _excelOrdersDto[i] = await GetOrderWorksheetsAsync(_excelOrdersDto[i]);

                    for (int j = 0; j < _excelOrdersDto[i].Files.Count; j++)
                    {
                        _progressValue.Value++;
                        _progress?.Report(_progressValue);

                        for (int k = 0; k < _excelOrdersDto[i].Files[j].Worksheets.Count; k++)
                        {
                            _progressValue.ProgressTask2 = $"Worksheet #{_excelOrdersDto[i].Files[j].Worksheets[k].Name}";

                            SourceAppType sourceAppType = _excelOrdersDto[i].SourceAppType;
                            WorksheetType worksheetType = _excelOrdersDto[i].Files[j].Worksheets[k].WorksheetType;

                            if (sourceAppType == SourceAppType.Unknown)
                            {
                                _logger.LogError("Excel files format not recognized. Source application is Unknown. File {$Filename}."
                                , _excelOrdersDto[i].Files[j].FileName);
                                continue;
                            }

                            if (worksheetType == WorksheetType.Unknown)
                            {
                                _logger.LogError(" Excel files format not recognized. Worksheet type  is Unknown. File {$Filename}.",
                                  _excelOrdersDto[i].Files[j].FileName);
                                _excelOrdersDto[i].ErrorsDto.Add(new ErrorEntity
                                {
                                    OrderNumber = _excelOrdersDto[i].OrderNumber,
                                    Level = ErrorLevel.Fatal,
                                    Code = ErrorCode.Excel_Read_Workbook_Source_Application_Format_Unknown,
                                    Message = $"OrderNumber {_excelOrdersDto[i].OrderNumber}. File ${_excelOrdersDto[i].Files[j].FileName} contains worksheet with unknown type."
                                });
                                continue;
                            }

                            //=====================================================================================================`
                            //🔵 TechnoDesign Items
                            //=====================================================================================================
                            if (sourceAppType == SourceAppType.TechDesign || worksheetType == WorksheetType.Items)
                            {
                                List<ItemDto> result = await _excelParserTechDesign.MapItemsAsync(_excelOrdersDto[i].Files[j].Worksheets[k], _progressValue, _progress);
                                _excelOrdersDto[i].ItemsDto.AddRange(result);
                            }

                            //=====================================================================================================`
                            //🔵 Schuco Items
                            //=================================`====================================================================
                            else if (sourceAppType == SourceAppType.Schuco || worksheetType == WorksheetType.Items)
                            {
                                List<ItemDto> result = await _excelParserSchuco.MapItemsAsync(_excelOrdersDto[i].Files[j].Worksheets[k], _progressValue, _progress);
                                _excelOrdersDto[i].ItemsDto.AddRange(result);
                            }

                            //=====================================================================================================`
                            //🔵 TechnoDesign Materals
                            //=====================================================================================================
                            else if (sourceAppType == SourceAppType.TechDesign || worksheetType == WorksheetType.Materials)
                            {
                                List<MaterialDto> result = await _excelParserTechDesign.MapMaterialsAsync(_excelOrdersDto[i].Files[j].Worksheets[k], _progressValue, _progress);
                                _excelOrdersDto[i].MaterialsDto.AddRange(result);
                            }
                            //=====================================================================================================`
                            //🔵 TechnoDesign Materals
                            //=====================================================================================================
                            else if (sourceAppType == SourceAppType.Schuco || worksheetType == WorksheetType.Materials)
                            {
                                List<ItemDto> result = await _excelParserSchuco.MapItemsAsync(_excelOrdersDto[i].Files[j].Worksheets[k], _progressValue, _progress);
                                _excelOrdersDto[i].ItemsDto.AddRange(result);
                            }

                            //=======================================================================================
                            //🔵 Materials Worksheet
                            //=======================================================================================
                            else
                            {
                                //=======================================================================================
                                //🔵 Unknown Materials
                                //=======================================================================================
                                //🔵TechnoDesign Materials
                                //=======================================================================================
                                //🔵 Schuco Materials
                                //=======================================================================================

                                if (_excelOrdersDto[i].SourceAppType == SourceAppType.Unknown)
                                {
                                    continue;
                                }
                                else if (_excelOrdersDto[i].SourceAppType == SourceAppType.TechDesign)
                                {
                                    List<MaterialDto> result = await _excelParserTechDesign.MapMaterialsAsync(_excelOrdersDto[i].Files[j].Worksheets[k], _progressValue, _progress);
                                    _excelOrdersDto[i].MaterialsDto.AddRange(result);

                                }
                                else if (_excelOrdersDto[i].SourceAppType == SourceAppType.Schuco)
                                {
                                    List<MaterialDto> result = await _excelParserSchuco.MapMaterialsAsync(_excelOrdersDto[i].Files[j].Worksheets[k], _progressValue, _progress);
                                    _excelOrdersDto[i].MaterialsDto.AddRange(result);
                                }
                                else
                                {
                                    _logger.LogError("Excel files format not recognized. Source application is Unknown. File {$Filename}.", _excelOrdersDto[i].Files[j].FileName);
                                    continue;
                                }

                            }

                        }
                    }
                }
                return await Task.Run(() => _excelOrdersDto);
            }
            catch (Exception ex)
            {

                _logger.LogError("PrefSuite Service: Unhandled error reading orders. Exception {$Exception}", ex.Message);
                return _excelOrdersDto;
            }

        }

        private async Task<List<OrderDto>?> GetOrders(List<string>? files)
        {

            if (files == null || files.Count == 0)
            {
                return await Task.Run(() => _excelOrdersDto);
            }

            try
            {
                List<string> workingFiles = files.Select(f => f)
                                      .Where(f => !f.Contains("~$") && f.EndsWith(".xlsx")).ToList();

                if (workingFiles == null || workingFiles.Count == 0)
                {
                    return await Task.Run(() => _excelOrdersDto);
                }

                List<string> orderNumbers = workingFiles.Select(f => Path.GetFileName(f)!.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0])
                      .Distinct()
                      .OrderBy(f => f)
                      .ToList();

                foreach (string orderNumber in orderNumbers)
                {

                    OrderDto orderDto = new()
                    {
                        OrderNumber = orderNumber,
                        ItemsDto = [],
                        MaterialsDto = [],
                        ErrorsDto = [],
                        Currency = "Unknown",
                        ExchangeRate = 1

                    };

                    _excelOrdersDto.Add(orderDto);

                }

                return await Task.Run(() => _excelOrdersDto);

            }

            catch (Exception ex)
            {
                _logger.LogError($"Error in GetOrders: {ex.Message}");
                return await Task.Run(() => _excelOrdersDto);

            }
        }

        private async Task<OrderDto> GetOrderFilesAsync(OrderDto order)
        {

            try
            {
                await Task.Run((Action)(() =>
                {

                    List<string> files = _fileService.GetFiles().Select(f => f)
                        .Where(f => !f.Contains("~$") && f.EndsWith(".xlsx") && Path.GetFileName(f)!.StartsWith(order.OrderNumber)).ToList();

                    for (int i = 0; i < files.Count; i++)
                    {

                        Application.Models.File file = new()
                        {
                            FullName = files[i],
                            OrderNumber = order.OrderNumber,
                            FileName = System.IO.Path.GetFileName(files[i]),
                            // IsLocked = _fileService.IsLocked(files[i]),
                            FilePath = System.IO.Path.GetFullPath(files[i]),
                            Worksheets = []
                        };

                        if (file.IsLocked)
                        {

                            //order.Errors.Add(new ErrorEntity
                            //{
                            //    OrderNumber = order.OrderNumber,
                            //    Level = ErrorLevel.Fatal,
                            //    Code = ErrorCode.FileSystemReadWrite,
                            //    Message = $"OrderNumber {order.OrderNumber}. File ${file.FileName} is Locked."

                            //});
                        }
                        //order.Files.Add(file);
                    }

                }));
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetOrderFiles: {ex.Message}");
                return order;
            }
        }

        private async Task<OrderDto> GetOrderWorksheetsAsync(OrderDto order)
        {
            try
            {
                for (int i = 0; i < order.Files.Count; i++)
                {

                    List<Worksheet> worksheets = await _excelService.GetWorksheetsAsync(order.Files[i], _progressValue, _progress);

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
                _logger.LogError($"Error in GetOrderWorksheets: {ex.Message}");
                return order;
            }
        }

        private async Task<OrderDto> GetOrderSalesDocumentAsync(OrderDto order)
        {

            (int, int) salesDocument = (-1, -1);
            try
            {

                (int?, int?) result = await _prefSuiteDataService.GetSalesDocumentAsync(order.OrderNumber);
                if (result.Item1 < 1 || result.Item2 < 1)
                {
                    salesDocument.Item1 = -1;
                    salesDocument.Item2 = -1;

                    //order.Errors.Add(new ErrorEntity
                    //{
                    //    OrderNumber = order.OrderNumber,
                    //    Level = ErrorLevel.Fatal,
                    //    Code = ErrorCode.DatabaseRead_OrderReferenceNotFound,
                    //    Message = $"OrderNumber# {order.OrderNumber} not exists in PrefSuite DB !"
                    //}
                    //);
                }

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

                //order.SalesDocumentState = await _prefSuiteDataService.GetSalesDocumentStateAsync(order.Number, order.Version);

                return order;
            }
            catch (Exception)
            {
                // _logger.LogError("Unhandled error {$Class}.{Method}." +
                //     "\nOrder {$OrderNumber}." +
                //     " \n{$Exception}",
                //nameof(ReadService),
                //     nameof(GetOrderSalesDocumentAsync),
                //     order.OrderNumber ?? string.Empty,
                //     ex.Message);

                return order;
            }

        }
        private OrderDto SetSalesDocumentReadErrors(OrderDto order)
        {
            try
            {
                SalesDocument salesDocument = new();
                //{
                //    SalesDocumentNumber = order.SalesDocumentNumber,

                ///  OrderState orderState = (OrderState)order.SalesDocumentState;

                // if (orderState.HasFlag(OrderState.PurchaseOrdersExist))
                //{
                //    //order.Errors.Add(new ErrorEntity
                //    //{
                //    //    OrderNumber = order.OrderNumber,
                //    //    Level = ErrorLevel.Fatal,
                //    //    Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                //    //    Message = $"OrderNumber {order.OrderNumber} - {order.Number}/{order.Version} contains purchase orders.\nPlease remove purchase orders and try load data again."

                //    //});
                //    return order;

                //}

                //  if (orderState.HasFlag(OrderState.MaterialNeedsInserted))
                //    {
                //order.Errors.Add(new ErrorEntity
                //{
                //    OrderNumber = order.OrderNumber,
                //    Level = ErrorLevel.Warning,
                //    Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                //    Message = $"OrderNumber {order.OrderNumber} - {order.Number}/{order.Version} contains calculated material needs!"

                //});

                //     }
                //
                //if (orderState.HasFlag(OrderState.ItemsCreated))
                //{
                //    order.Errors.Add(new ErrorEntity
                //    {
                //        OrderNumber = order.OrderNumber,
                //        Level = ErrorLevel.Warning,
                //        Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                //        Message = $"OrderNumber {order.OrderNumber} - {order.Number}/{order.Version} contains PrefSuite items!"

                //    });
                //    return order;

                //}

                //if (orderState.HasFlag(OrderState.A2PItemsImported))
                //{
                //    //order.Errors.Add(new ErrorEntity
                //    //{
                //    //    OrderNumber = order.OrderNumber,
                //    //    Level = ErrorLevel.Warning,
                //    //    Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                //    //    Message = $"OrderNumber {order.OrderNumber} - {order.Number}/{order.Version} contains imported items worksheet!"

                //    //});

                //}

                //if (orderState.HasFlag(OrderState.A2PMaterialsImported))
                //{
                //    //order.Errors.Add(new ErrorEntity
                //    //{
                //    //    OrderNumber = order.OrderNumber,
                //    //    Level = ErrorLevel.Warning,
                //    //    Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                //    //    Message = $"OrderNumber {order.OrderNumber} - {order.Number}/{order.Version} contains imported material worksheets!"

                //    //});

                //}

                return order;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                //    _logger.LogError("Unhandled error {$Class}.{Method}." +
                //        "\nOrder {$OrderNumber}." +
                //        " \n{$Exception}",
                //   nameof(ReadService),
                //        nameof(GetOrderSalesDocumentAsync),
                //        order.OrderNumber ?? string.Empty,
                //        ex.Message);

                return order;
            }

        }

    }
}
