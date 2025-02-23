using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Application.Services.Domain.Enums;
using a2p.Shared.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Extensions.Configuration;

namespace a2p.Shared.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;
        private readonly IExcelReadService _excelReadService;
        private readonly IPrefSuiteService _prefSuiteService;
        private readonly IMapperSapaV1 _mapperSapaV1;
        private readonly IMapperSapaV2 _mapperSapaV2;
        private readonly IMapperSchuco _mapperSchuco;

        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public FileService(IConfiguration configuration,
                           ILogService logService,
                           IExcelReadService excelReadService,
                           IPrefSuiteService prefSuiteService,
                            IMapperSapaV1 mapperSapaV1,
                            IMapperSapaV2 mapperSapaV2,
                            IMapperSchuco mapperSchuco)

        {
            _configuration = configuration;
            _logService = logService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _excelReadService = excelReadService;

            _prefSuiteService = prefSuiteService;
            _mapperSapaV1 = mapperSapaV1;
            _mapperSapaV2 = mapperSapaV2;
            _mapperSchuco = mapperSchuco;

        }

        public async Task<List<A2POrder>> GetOrdersAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress;
            List<A2POrder> a2pOrders = await GetOrderNumbersAsync();

            _progressValue.ProgressTask1 = $"Found {a2pOrders.Count} a2pOrders ";
            _progressValue.Value = 0;
            _progressValue.MinValue = 0;
            _progressValue.MaxValue = a2pOrders.Count * 2;

            int progressBarValue = 0;
            int ordersCounter = 0;

            //Iterate through a2pOrders
            //===================================================================
            for (int i = 0; i < a2pOrders.Count; i++)
            {
                progressBarValue++;
                ordersCounter++;
                _progressValue.ProgressTask1 = $"Reading Order # {ordersCounter} of {a2pOrders.Count} - {a2pOrders[i].Order} ";
                _progress?.Report(_progressValue);
                a2pOrders[i].Files = await GetOrderFilesAsync(a2pOrders[i].Order);

                (int, int) salesDocument = await GetSalesDocumentAsync(a2pOrders[i].Order);
                a2pOrders[i].SalesDocNumber = salesDocument.Item1;
                a2pOrders[i].SalesDocVersion = salesDocument.Item2;

                a2pOrders[i].OrderExists = await OrderExists(a2pOrders[i].Order);
                if (a2pOrders[i].OrderExists.HasValue)
                {
                    a2pOrders[i].ReadErrors.Add(new A2POrderError
                    {
                        Order = a2pOrders[i].Order,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.DatabaseRead_Order,
                        Message = $"Order {a2pOrders[i]} exists. Imported on {a2pOrders[i].OrderExists}."

                    });
                }

                int fileCounter = 0;
                //Iterate through files
                //===================================================================

                for (int j = 0; j < a2pOrders[i].Files.Count; j++)
                {

                    fileCounter++;

                    _progressValue.ProgressTask2 = $"Reading File {fileCounter} of {a2pOrders[i].Files.Count()} - {a2pOrders[i].Files[j].FileName}";
                    _progress?.Report(_progressValue);
                    if (a2pOrders[i].Files[j].FileName.Contains(" "))
                    {
                        a2pOrders[i].SourceAppType = SourceAppType.SapaV2;

                        if (a2pOrders[i].Files[j].FileName.Contains("Price_Details"))
                        {
                            a2pOrders[i].Files[j].IsOrderItemsFile = true;
                        }

                    }
                    if (a2pOrders[i].Files[j].IsLocked)
                    {
                        _logService.Warning("File Service: File is locked {$FilePath}", a2pOrders[i].Files[j].FilePath);
                        a2pOrders[i].ReadErrors.Add(new A2POrderError
                        {
                            Order = a2pOrders[i].Order,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.FileSystemRead,
                            Message = $"Order {a2pOrders[i]} file {a2pOrders[i].Files[j].FileName} is locked by other application! "

                        });

                        a2pOrders[i].Files[j].Order = a2pOrders[i].Order;
                    }

                    List<A2PWorksheet> a2pWorksheets = await _excelReadService.GetWorksheetsAsync(a2pOrders[i].Files[j], _progressValue, _progress);

                    a2pOrders[i].Files[j].Worksheets = a2pWorksheets;

                    int worksheetCounter = 0;

                    for (int k = 0; k < a2pOrders[i].Files[j].Worksheets.Count; k++)
                    {
                        worksheetCounter++;

                        a2pOrders[i].Files[j].Worksheets[k].Order = a2pOrders[i].Order;

                        _progressValue.ProgressTask2 = $"Reading worksheet {worksheetCounter} of {a2pOrders[i].Files[j].Worksheets.Count} - {a2pOrders[i].Files[j].Worksheets[k].Name}.";
                        _progress?.Report(_progressValue);

                        //Sapa V1 Mapping 
                        //===================================================================
                        if (a2pOrders[i].Files[j].IsOrderItemsFile && a2pOrders[i].SourceAppType == SourceAppType.SapaV1)

                        {
                            a2pOrders[i].Items = await _mapperSapaV1.MapItemsAsync(a2pOrders[i].Files[j].Worksheets[k], _progressValue, _progress);
                        }
                        else if (!a2pOrders[i].Files[j].IsOrderItemsFile && a2pOrders[i].SourceAppType == SourceAppType.SapaV1)
                        {
                            a2pOrders[i].Materials = await _mapperSapaV1.MapMaterialsAsync(a2pOrders[i].Files[j].Worksheets[k], _progressValue, _progress);
                        }

                        //Sapa V2 Mapping 
                        //===================================================================
                        else if (a2pOrders[i].Files[j].IsOrderItemsFile && a2pOrders[i].SourceAppType == SourceAppType.SapaV2)
                        {
                            a2pOrders[i].Items = await _mapperSapaV2.MapItemsAsync(a2pOrders[i].Files[j].Worksheets[k], _progressValue, _progress);
                            if (string.IsNullOrEmpty(a2pOrders[i].Currency) &&
                                (!string.IsNullOrEmpty(a2pOrders[i].Files[j].Worksheets[k].Currency)))
                            {
                                a2pOrders[i].Currency = a2pOrders[i].Files[j].Worksheets[k].Currency;
                            }
                        }
                        else if (!a2pOrders[i].Files[j].IsOrderItemsFile && a2pOrders[i].SourceAppType == SourceAppType.SapaV2)
                        {
                            a2pOrders[i].Materials = await _mapperSapaV2.MapMaterialsAsync(a2pOrders[i].Files[j].Worksheets[k], _progressValue, _progress);
                        }

                        //Schuco Mapping
                        //===================================================================
                        else if (a2pOrders[i].Files[j].IsOrderItemsFile && a2pOrders[i].SourceAppType == SourceAppType.Schuco)
                        {
                            a2pOrders[i].Items = await _mapperSchuco.MapItemsAsync(a2pOrders[i].Files[j].Worksheets[k], _progressValue, _progress);
                        }
                        else if (!a2pOrders[i].Files[j].IsOrderItemsFile && a2pOrders[i].SourceAppType == SourceAppType.Schuco)
                        {
                            a2pOrders[i].Materials = await _mapperSchuco.MapMaterialsAsync(a2pOrders[i].Files[j].Worksheets[k], _progressValue, _progress);
                        }

                    }

                }

                progressBarValue++;

            }

            return a2pOrders;

        }

        private async Task<DateTime?> OrderExists(string orderNumber)
        {

            string? materialsDateTime = await _prefSuiteService.MaterialsExistsAsync(orderNumber);
            string? itemsDateTime = await _prefSuiteService.ItemsExistsAsync(orderNumber);

            DateTime? materialsDate = !string.IsNullOrEmpty(materialsDateTime) ? DateTime.Parse(materialsDateTime) : (DateTime?) null;
            DateTime? itemsDate = !string.IsNullOrEmpty(itemsDateTime) ? DateTime.Parse(itemsDateTime) : (DateTime?) null;

            DateTime? mostRecentDate = null;

            if (materialsDate.HasValue && itemsDate.HasValue)
            {
                mostRecentDate = materialsDate > itemsDate ? materialsDate : itemsDate;
            }
            else if (materialsDate.HasValue)
            {
                mostRecentDate = materialsDate;
            }
            else if (itemsDate.HasValue)
            {
                mostRecentDate = itemsDate;
            }

            if (mostRecentDate.HasValue)
            {
                _logService.Information("File Service: Order {$Order} items already imported on {$Date}.", orderNumber, mostRecentDate);
            }

            return mostRecentDate;
        }

        private async Task<(int, int)> GetSalesDocumentAsync(string orderNumber)
        {

            //SAet sales doc number and version if order exists
            //===================================================================
            (int, int) salesDoc = await _prefSuiteService.GetSalesDocAsync(orderNumber);

            return salesDoc;

        }
        private async Task<bool> IsLockedAsync(string filePath)
        {
            try

            {
                using FileStream stream = await Task.Run(() => new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                return false;
            }
            catch (Exception ex)
            {
                _logService.Warning("File Service: File locked ${FilePath}. Reason: ${Exception}", filePath, ex.Message);
            }
            return true;
        }
        // Move files asynchronously
        public void MoveFilesAsync(string source, string destination)
        {
            try
            {
                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }

                File.Move(source, destination);

            }

            catch (Exception ex)
            {
                _logService.Fatal(ex, "File Service: Unhandled error moving files");
            }
        }

        private async Task<List<A2POrder>> GetOrderNumbersAsync()
        {

            List<A2POrder> orders = [];

            string rootFolder = _configuration["AppSettings:RootFolder"] ?? @"C:\Temp\Import";
            List<string> files = (await Task.Run(() => Directory.GetFiles(rootFolder))).ToList(); // Get all files in the root folder
            if (files == null || !files.Any())
            {
                _logService.Information("File Service: No files found in {$RootFolder}", rootFolder);
                return [];
            }

            List<string> orderNumbers = files.Where(o => o != null && !o.Contains("~$"))
                .Select(o => Path.GetFileName(o)!.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0])
                .Distinct()
                .OrderBy(o => o)
                .ToList();

            if (orderNumbers == null || !orderNumbers.Any())
            {
                _logService.Information("File Service: No a2pOrders found in {$RootFolder}", rootFolder);
                return [];
            }

            foreach (string orderNumber in orderNumbers)
            {
                A2POrder order = new() { Order = orderNumber };
                orders.Add(order);
            }
            return orders;
        }

        private async Task<List<A2PFile>> GetOrderFilesAsync(string orderNumber)
        {
            string rootFolder = _configuration["AppSettings:RootFolder"] ?? @"C:\Temp\Import";
            List<string> files = (await Task.Run(() => Directory.GetFiles(rootFolder))).ToList(); // Get all files in the root folder
            List<string> orderFiles = files.Where(file => file.Contains(orderNumber)).ToList(); // Get all files that match the order number

            List<A2PFile> a2pFiles = [];

            for (int i = 0; i < orderFiles.Count; i++)
            {
                A2PFile a2pFile = new()
                {
                    Order = orderNumber,
                    File = orderFiles[i],
                    IsLocked = await IsLockedAsync(orderFiles[i]),
                    FilePath = Path.GetDirectoryName(orderFiles[i]) ?? string.Empty,
                    FileName = Path.GetFileName(orderFiles[i]) ?? string.Empty
                };
                a2pFiles.Add(a2pFile);

            }
            return a2pFiles;
        }

        Task<bool> IFileService.IsLockedAsync(string filePath)
        {
            return IsLockedAsync(filePath);
        }
    }
}