using a2p.Shared.Application.Interfaces;
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
        private readonly IPrefSuiteIntegrationService _prefSuiteIntegrationService;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;




        public FileService(IConfiguration configuration,
                           ILogService logService,
                           IExcelReadService excelReadService,
                           IPrefSuiteIntegrationService prefSuiteIntegrationService)
        {
            _configuration = configuration;
            _logService = logService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _excelReadService = excelReadService;

            _prefSuiteIntegrationService = prefSuiteIntegrationService;
        }

        //// Get orders and files asynchronously
        //public async Task<List<A2POrder>> GetOrdersAsync(List<A2POrder> a2pOrderList, ProgressValue progressValue, IProgress<ProgressValue>? progress)
        //{
        //    _progressValue = progressValue;
        //    _progressValue.ProgressTitle = $"Getting Folder...";
        //    progress?.Report(_progressValue);
        //    string rootFolder = _configuration["AppSettings:RootFolder"] ?? @"C:\Temp\Import";


        //    // Get file names asynchronously
        //    //=======================================================================================================================================


        //    //progress searching files
        //    {
        //        _progressValue.ProgressTitle = $"Searching Files in {rootFolder}...";
        //        progress?.Report(_progressValue);

        //    }
        //    List<string> files = (await Task.Run(() => Directory.GetFiles(rootFolder))).ToList(); // Get all files in the root folder

        //    //progress found files
        //    {

        //        _progressValue.ProgressTitle = "Searching Orders...";
        //        _progressValue.ProgressTask1 = $"Found {files.Count}) file in {rootFolder}.";
        //        progress?.Report(_progressValue);


        //    }



        //    // Extract unique orders numbers
        //    //=======================================================================================================================================
        //    if (files == null || !files.Any())
        //    {
        //        _logService.Information("File Service: No files found in {RootFolder}", rootFolder);
        //        return a2pOrderList;
        //    }
        //    IEnumerable<string?> fileNames = files.Select(Path.GetFileName).ToList();
        //    IEnumerable<string> orderNumbers = fileNames
        //     .Where(o => o != null && !o.Contains("~$"))
        //     .Select(o => o!.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0])
        //     .Distinct()
        //     .OrderBy(o => o)
        //     .ToList();

        //    //progress found orders
        //    {
        //        _progressValue.ProgressTask1 = $"Found {orderNumbers.Count()} Orders in files...";

        //        progress?.Report(_progressValue);
        //    }
        //    int count = 0;
        //    int progresValueCount = 0;
        //    foreach (string orderNumber in orderNumbers)
        //    {

        //        progresValueCount += 1;

        //        {
        //            _progressValue.ProgressTitle = $"Processing Order # {orderNumber}. Order {count + 1} / {orderNumbers.Count()}";
        //            _progressValue.MaxValue = orderNumbers.Count() * 2;
        //            _progressValue.Value = progresValueCount;
        //            _progressValue.ProgressTask1 = $"Searching Files...";
        //            _progressValue.ProgressTask2 = string.Empty;
        //            _progressValue.ProgressTask3 = string.Empty;
        //            progress?.Report(_progressValue);
        //        }
        //        A2POrder a2pOrder = new()
        //        {
        //            Order = orderNumber,
        //            Files = await GetSingleOrderFilesAsync(orderNumber, files, progress)
        //        };

        //        foreach (A2PFile file in a2pOrder.Files)
        //        {

        //            if (file.IsLocked)
        //            {
        //                _logService.Error("File Service : Order: {$Order}, file {$FileName} is locked.", a2pOrder.Order, file.FileName);
        //                a2pOrder.ReadErrors.Add(new A2POrderError
        //                {
        //                    Order = a2pOrder.Order,
        //                    Level = ErrorLevel.Error,
        //                    Code = ErrorCode.File_System_Read,
        //                    Description = $"Order: {a2pOrder.Order}, file ${file.FileName} is locked."
        //                });
        //                continue;
        //            }


        //            if (file.Worksheets == null || !file.Worksheets.Any())
        //            {
        //                _logService.Warning("File Seervice: No worksheets found in {FileName}", file.FileName);

        //                a2pOrder.ReadErrors.Add(new A2POrderError
        //                {
        //                    Order = a2pOrder.Order,
        //                    Level = ErrorLevel.Error,
        //                    Code = ErrorCode.ReadService_WorksheetRead,
        //                    Description = $"No worksheets found in Order :{orderNumber} file {file.FileName}, file removed"
        //                });

        //                continue;
        //            }
        //        }

        //       

        //        _ = await A2POrderAgregator.AddOrUpdateOrderAsync(a2pOrderList, a2pOrder);

        //        progresValueCount += 1;
        //        _progressValue.Value = progresValueCount;
        //        progress?.Report(_progressValue);
        //        count++;

        //    }
        //    return a2pOrderList;
        //}



        public async Task<IEnumerable<A2POrder>> GetOrdersAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress;
            List<A2POrder> orders = [];

            string rootFolder = _configuration["AppSettings:RootFolder"] ?? @"C:\Temp\Import";
            List<string> files = (await Task.Run(() => Directory.GetFiles(rootFolder))).ToList(); // Get all files in the root folder

            if (files == null || !files.Any())
            {
                _logService.Information("File Service: No files found in {$RootFolder}", rootFolder);
                return orders;
            }


            IEnumerable<string?> ordersFiles = files.Select(Path.GetFileName).ToList();
            IEnumerable<string> distictOrders = ordersFiles
             .Where(o => o != null && !o.Contains("~$"))
             .Select(o => o!.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0])
             .Distinct()
             .OrderBy(o => o)
             .ToList();


            foreach (string distinctOrder in distictOrders)

            {
                A2POrder order = new()
                {
                    Order = distinctOrder

                };

                //et sales doc number and version if order exists
                //===================================================================
                (int, int) salesDoc = await _prefSuiteIntegrationService.GetSalesDocAsync(order.Order);
                order.SalesDocNumber = salesDoc.Item1;
                order.SalesDocVersion = salesDoc.Item2;

                if (order.SalesDocNumber <= 0 || order.SalesDocVersion <= 0)
                {
                    _logService.Warning("File Service : Order # {Order} not exists in PrefSuite.", order.Order);
                    order.ReadErrors.Add(new A2POrderError
                    {
                        Order = order.Order,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.ReadService_OrderNotExistInPrefSuite,
                        Description = $"Order: {order.Order} not exists in PrefSuite."
                    });
                }

                // Check if materials already exists
                //===================================================================


                string? itemsExistsInDB = await _prefSuiteIntegrationService.MaterialsExistsAsync(order.Order);
                if (!string.IsNullOrEmpty(itemsExistsInDB))
                {
                    _logService.Warning("File Service: Order {$Order} items already imported on {$Date}.", order.Order, itemsExistsInDB);
                    if (DateTime.TryParse(itemsExistsInDB, out DateTime itemsExistsDate))
                    {
                        order.OrderExists = itemsExistsDate;
                    }
                }
                string? materialsExistsInDB = await _prefSuiteIntegrationService.MaterialsExistsAsync(order.Order);
                if (!string.IsNullOrEmpty(materialsExistsInDB))
                {
                    _logService.Warning("File Service: Order {$Order} items already imported on {$Date}.", order.Order, materialsExistsInDB);

                    if (DateTime.TryParse(materialsExistsInDB, out DateTime materialsExistsDate))
                    {
                        order.OrderExists = materialsExistsDate;
                    }
                }

                if (!string.IsNullOrEmpty(order.OrderExists.ToString()))
                {
                    order.ReadErrors.Add(new A2POrderError
                    {
                        Order = order.Order,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.SQLRepository_OrderAlreadyExistsInDB,
                        Description = $"Order with same number {order.Order} already exists in SQL DB. Last import known date  {order.OrderExists}"
                    });
                }

                order = await GetOrderFilesAsync(order, progressValue, progress);
                orders.Add(order);
            }

            return orders;

        }


        // Get files for a specific order asynchronously
        //=================================================================================================================================
        public async Task<A2POrder> GetOrderFilesAsync(A2POrder order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            string rootFolder = _configuration["AppSettings:RootFolder"] ?? @"C:\Temp\Import";

            // Get all file in folder 
            List<string> fileList = (await Task.Run(() => Directory.GetFiles(rootFolder))).ToList(); // Get all files in the root folder
            
            //Filtering files that match order number
            List<string>? matchingFiles = fileList.Where(file => file.Contains(order.Order)).ToList();


            // Check if files exists
            if (matchingFiles == null || !matchingFiles.Any())
            {
                _logService.Warning("File Service: Files not found!  Order: {$Order}, {Folder}", order.Order, rootFolder);
                return order;
            }


            foreach (string matchingFile in matchingFiles)
            {

                A2PFile a2pFile = new()
                {
                    File = matchingFile,
                    IsLocked = await IsLockedAsync(matchingFile),
                    FilePath = Path.GetDirectoryName(matchingFile) ?? string.Empty,
                    FileName = Path.GetFileName(matchingFile) ?? string.Empty
                };

                if (a2pFile.FileName.Contains(" "))
                {
                    order.SourceAppType = SourceAppType.SapaV2;
                }
                if (a2pFile.FileName.Contains("Price_Details"))
                {
                    a2pFile.IsOrderItemsFile = true;

                }
                if (a2pFile.IsLocked == true)
                {
                    _logService.Warning("File Service: File is locked {$FilePath}", a2pFile.FilePath);
                    order.ReadErrors.Add(new A2POrderError
                    {
                        Order = order.Order,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.File_System_Read,
                        Description = $"File is locked {a2pFile.FilePath}"
                    });
                
                }
                order = await _excelReadService.GetWorksheetsAsync(order, a2pFile, progressValue, progress);
            }

            return order;              

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

        public Task<A2POrder> GetOrderFilesAsync(A2POrder order, IProgress<ProgressValue>? progress = null)
        {
            throw new NotImplementedException();
        }

        Task<bool> IFileService.IsLockedAsync(string filePath)
        {
            return IsLockedAsync(filePath);
        }
    }
}