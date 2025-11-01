using Application.DTOs;
using Application.Interfaces.Excel;
using Application.Interfaces.Files;
using Application.Interfaces.Orchestrators;
using Application.Interfaces.Services;
using Application.Models;

using Domain.Enums;

using Microsoft.Extensions.Logging;

using System.Data;

namespace Infrastructure.Services.Orchestartors
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
                //===================================================================================================================================================================================================================================================================================================================
                //🔵 Get files list
                //===================================================================================================================================================================================================================================================================================================================
                List<string> files = _fileService.GetLocalFiles();
                if (files.Count == 0)
                {

                    return [];

                }

                //===================================================================================================================================================================================================================================================================================================================
                //🔵 Get Orders list from file list
                //===================================================================================================================================================================================================================================================================================================================
                List<string> orders = GetOrders(files);

                //===================================================================================================================================================================================================================================================================================================================
                //🔵 Create OrdersDto List 
                //===================================================================================================================================================================================================================================================================================================================

                for (int i = 0; i < orders.Count; i++)
                {
                    List<string> excelFiles = GetOrderFiles(orders[i], files);
                    OrderDto orderDto = new();

                    orderDto.OrderNumber = orders[i];
                    orderDto.ItemsDto = [];
                    orderDto.MaterialsDto = [];
                    orderDto.ExcelFiles = [];

                    for (int j = 0; j < excelFiles.Count; j++)
                    {
                        ExcelFile excelFile = new();

                        excelFile.FilePath = excelFiles[j];
                        excelFile.FileName = Path.GetFileName(excelFiles[j]);
                        excelFile.IsLocked = _fileService.IsLocked(excelFiles[j]);
                        orderDto.ExcelFiles.Add(excelFile);
                    }
                    _ordersDto.Add(orderDto);
                }

                //===================================================================================================================================================================================================================================================================================================================
                //🔵 Initialize Progress Bar
                //===================================================================================================================================================================================================================================================================================================================
                _progressValue.Value = 0;

                _progressValue.MaxValue = _ordersDto.Count * 5;
                _progressValue.ProgressTask1 = $"Found {_ordersDto.Count} orders!";
                _progress?.Report(_progressValue);

                //===================================================================================================================================================================================================================================================================================================================
                //🔵 Get OrderNumber from local files
                //===================================================================================================================================================================================================================================================================================================================

                for (int i = 0; i < _ordersDto.Count; i++)
                {
                    _progressValue.Value++;
                    _progressValue.ProgressTask1 = string.Empty;
                    _progressValue.ProgressTask2 = $"Searching Orders ExcelFiles {i + 1} of {_ordersDto.Count} - OrderNumber #{_ordersDto[i].OrderNumber}";
                    _progress?.Report(_progressValue);

                    if (string.IsNullOrEmpty(_ordersDto[i].OrderNumber))
                    {
                        _logger.LogError("PrefSuite Service: OrderNumber is null or empty. Skipping order.");
                        continue;

                    }

                    // Get Sales Document 
                    SalesDocument? salesDocument = await _prefSuiteDataService.GetSalesDocumentByOrderNumberAsync(_ordersDto[i].OrderNumber!);

                    if (salesDocument != null)
                    {
                        _ordersDto[i].SalesDocument = salesDocument;
                        _ordersDto[i].Id = salesDocument.RowId ?? Guid.Empty;

                    }
                    _ordersDto[i] = await GetOrderWorksheetsAsync(_ordersDto[i]);
                    _ordersDto[i] = await GetOrderItemsAsync(_ordersDto[i]);
                    _ordersDto[i] = await GetOrderMaterialsAsync(_ordersDto[i]);
                    _ordersDto[i] = await SetSalesDocumentReadErrors(_ordersDto[i]);

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
                 .Select(f => Path.GetFileName(f).Split(' ', '_')[0]) // Assuming orderDto number is before the first underscore
                 .Distinct()
                 .ToList() ?? [];
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError("{$Class}.{$Method}. Unhandled error getting orders! Exception: {$Exception}",
                 nameof(ReadService),
                 nameof(GetOrders),
                 ex.Message);
                return [];
            }
        }

        private List<string> GetOrderFiles(string order, List<string> files)
        {
            try
            {

                List<string> orderFiles = files.Where(f => f.Contains(order) && !f.Contains("~$") && f.EndsWith(".xlsx")).ToList(); // Get all files that match the orderDto number

                return orderFiles ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError("{$Class}.{$Method}. Unhandled error getting ordr files! Exception: {$Exception}",
                 nameof(ReadService),
                 nameof(GetOrderFiles),
                 ex.Message);
                return [];
            }
        }

        private async Task<OrderDto> GetOrderWorksheetsAsync(OrderDto orderDto)
        {
            try
            {
                for (int i = 0; i < orderDto.ExcelFiles.Count; i++)
                {

                    List<Worksheet> worksheetsDto = await _excelService.ReadWorkbook(orderDto.ExcelFiles[i], _progressValue, _progress);

                    if (worksheetsDto == null)
                    {
                        continue;
                    }
                    orderDto.ExcelFiles[i].Worksheets.AddRange(worksheetsDto);
                }

                return orderDto;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetOrderWorksheets: {ex.Message}");
                return orderDto;
            }
        }

        private async Task<OrderDto> GetOrderItemsAsync(OrderDto orderDto)
        {
            try
            {

                for (int i = 0; i < orderDto.ExcelFiles.Count; i++)
                {

                    //Process Items worksheets
                    //=========================================================================================
                    for (int j = 0; j < orderDto.ExcelFiles[i].Worksheets.Count; j++)
                    {
                        SourceAppType sourceAppType = orderDto.ExcelFiles[i].Worksheets[j].SourceAppType;
                        WorksheetType worksheetType = orderDto.ExcelFiles[i].Worksheets[j].WorksheetType;

                        _progressValue.Value++;
                        _progress?.Report(_progressValue);

                        //====================================================================================================================================================================================================``
                        //🔵 TechnoDesign Items
                        //====================================================================================================================================================================================================
                        if (sourceAppType == SourceAppType.TechDesign && worksheetType == WorksheetType.Items)
                        {
                            List<ItemDto> itemsDto = await _excelParserTechDesign.ParseItemsAsync(orderDto.ExcelFiles[i].Worksheets[j], orderDto, _progressValue, _progress);

                            if (itemsDto == null || itemsDto.Count == 0)
                            {
                                continue;
                            }
                            orderDto.ItemsDto.AddRange(itemsDto);
                            orderDto.Currency = orderDto.ExcelFiles[i].Worksheets[i].Currency;

                        }

                        //====================================================================================================================================================================================================``
                        //🔵 Schuco Items
                        //====================================================================================================================================================================================================
                        if (sourceAppType == SourceAppType.Schuco && worksheetType == WorksheetType.Items)
                        {
                            List<ItemDto> itemsDto = await _excelParserSchuco.MapItemsAsync(orderDto.ExcelFiles[i].Worksheets[j], orderDto, _progressValue, _progress);
                            if (itemsDto == null || itemsDto.Count == 0)
                            {
                                continue;
                            }
                            orderDto.ItemsDto.AddRange(itemsDto);
                        }

                    }

                }
                return orderDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetOrderItems: {ex.Message}");
                return orderDto;
            }
        }

        private async Task<OrderDto> GetOrderMaterialsAsync(OrderDto orderDto)
        {
            try
            {

                for (int i = 0; i < orderDto.ExcelFiles.Count; i++)
                {

                    //Process Items worksheets
                    //=========================================================================================
                    for (int j = 0; j < orderDto.ExcelFiles[i].Worksheets.Count; j++)
                    {
                        SourceAppType sourceAppType = orderDto.ExcelFiles[i].Worksheets[j].SourceAppType;
                        WorksheetType worksheetType = orderDto.ExcelFiles[i].Worksheets[j].WorksheetType;

                        _progressValue.Value++;
                        _progress?.Report(_progressValue);

                        //====================================================================================================================================================================================================``
                        //🔵 TechnoDesign Materials
                        //====================================================================================================================================================================================================
                        if (sourceAppType == SourceAppType.TechDesign && worksheetType == WorksheetType.Materials)
                        {
                            List<MaterialDto> materialsDto = await _excelParserTechDesign.ParseMaterialsAsync(orderDto.ExcelFiles[i].Worksheets[j], orderDto, _progressValue, _progress);
                            if (materialsDto == null || materialsDto.Count == 0)
                            {
                                continue;
                            }

                            orderDto.MaterialsDto.AddRange(materialsDto);
                        }
                        //====================================================================================================================================================================================================``
                        //🔵 Schuco Materals
                        //====================================================================================================================================================================================================
                        if (sourceAppType == SourceAppType.Schuco && worksheetType == WorksheetType.Materials)
                        {

                            List<MaterialDto> result = await _excelParserTechDesign.ParseMaterialsAsync(orderDto.ExcelFiles[i].Worksheets[j], orderDto, _progressValue, _progress);
                            orderDto.MaterialsDto.AddRange(result);

                        }

                    }

                }
                return orderDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetOrderItems: {ex.Message}");
                return orderDto;
            }

        }
        private async Task<OrderDto> SetSalesDocumentReadErrors(OrderDto orderDto)
        {
            try
            {

                if (orderDto.SalesDocument.State.HasFlag(OrderState.PurchaseOrdersExist))
                {
                    _logger.LogError(@"Error processing order {$Order}. Sales Document {$Number}/{$Version} has purchase orders!. \nRemove all purchase orders and try again",
                    orderDto.OrderNumber, orderDto.SalesDocument.Number, orderDto.SalesDocument.Version);
                    return await Task.Run(() => orderDto);

                }

                if (orderDto.SalesDocument.State.HasFlag(OrderState.MaterialNeedsInserted))
                {
                    _logger.LogWarning(@"Warning processing order {$Order}. Sales Document {$Number}/{$Version} has material needs calculated!.\nIf you proceed, existing material needs will be deleted!",
                   orderDto.OrderNumber, orderDto.SalesDocument.Number, orderDto.SalesDocument.Version);
                    return await Task.Run(() => orderDto);

                }

                if (orderDto.SalesDocument.State.HasFlag(OrderState.ItemsCreated) || orderDto.SalesDocument.State.HasFlag(OrderState.A2PItemsImported))
                {
                    _logger.LogWarning(@"Warning processing order {$Order}. Sales Document {$Number}/{$Version} has items !.\nIf you proceed, existing items will be kept and new items will be inserted!",
                   orderDto.OrderNumber, orderDto.SalesDocument.Number, orderDto.SalesDocument.Version);
                    return await Task.Run(() => orderDto);

                }

                return await Task.Run(() => orderDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(@"{$Class}.{$Method}.Unhandled error getting sales document!\nOrder: {$Order},\nException: {$Exception}",
                 nameof(ReadService), nameof(SetSalesDocumentReadErrors), orderDto.OrderNumber, ex.Message);
                return orderDto;
            }

        }

    }
}
