using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Services;
using a2p.Shared.Infrastructure.Services.Other;

using Microsoft.Extensions.Configuration;

namespace a2p.Shared.Infrastructure.Services
{
    public class
     FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;
        private readonly IReadService _readService;
        private readonly IPrefService _prefService;
        private ProgressValue _progressValue;




        public FileService(IConfiguration configuration, ILogService logService, IReadService excelService, IPrefService prefService)
        {
            _configuration = configuration;
            _logService = logService;
            _readService = excelService;
            _progressValue = new ProgressValue();
            _prefService = prefService;
        }

        // Get orders and files asynchronously
        public async Task<List<A2POrder>> GetOrdersAsync(List<A2POrder> a2pOrderList, ProgressValue progressValue, IProgress<ProgressValue>? progress)
        {
            _progressValue = progressValue;
            _progressValue.ProgressTitle = $"Getting Folder...";
            progress?.Report(_progressValue);
            string rootFolder = _configuration["AppSettings:RootFolder"] ?? @"C:\Temp\Import";


            // Get file names asynchronously
            //=======================================================================================================================================


            //progress searching files
            {
                _progressValue.ProgressTitle = $"Searching Files in {rootFolder}...";
                progress?.Report(_progressValue);

            }
            List<string> files = (await Task.Run(() => Directory.GetFiles(rootFolder))).ToList(); // Get all files in the root folder

            //progress found files
            {

                _progressValue.ProgressTitle = "Searching Orders...";
                _progressValue.ProgressTask1 = $"Found {files.Count}) file in {rootFolder}.";
                progress?.Report(_progressValue);


            }



            // Extract unique orders numbers
            //=======================================================================================================================================
            if (files == null || !files.Any())
            {
                _logService.Information("File Service: No files found in {RootFolder}", rootFolder);
                return a2pOrderList;
            }
            IEnumerable<string?> fileNames = files.Select(Path.GetFileName).ToList();
            IEnumerable<string> orderNumbers = fileNames
             .Where(o => o != null && !o.Contains("~$"))
             .Select(o => o!.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries)[0])
             .Distinct()
             .OrderBy(o => o)
             .ToList();

            //progress found orders
            {
                _progressValue.ProgressTask1 = $"Found {orderNumbers.Count()} Orders in files...";

                progress?.Report(_progressValue);
            }
            int count = 0;
            int progresValueCount = 0;
            foreach (string orderNumber in orderNumbers)
            {

                progresValueCount += 1;

                {
                    _progressValue.ProgressTitle = $"Processing Order # {orderNumber}. Order {count + 1} / {orderNumbers.Count()}";
                    _progressValue.MaxValue = orderNumbers.Count() * 2;
                    _progressValue.Value = progresValueCount;
                    _progressValue.ProgressTask1 = $"Searching Files...";
                    _progressValue.ProgressTask2 = string.Empty;
                    _progressValue.ProgressTask3 = string.Empty;
                    progress?.Report(_progressValue);
                }
                A2POrder a2pOrder = new()
                {
                    Order = orderNumber,
                    Files = await GetSingleOrderFilesAsync(orderNumber, files, progress)
                };

                foreach (A2PFile file in a2pOrder.Files)
                {

                    if (file.IsLocked)
                    {
                        _logService.Error("File Service : Order: {$Order}, file {$FileName} is locked.", a2pOrder.Order, file.FileName);
                        a2pOrder.ReadErrors.Add(new A2POrderError
                        {
                            Order = a2pOrder.Order,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.File_System_Read,
                            Description = $"Order: {a2pOrder.Order}, file ${file.FileName} is locked."
                        });
                        continue;
                    }


                    if (file.Worksheets == null || !file.Worksheets.Any())
                    {
                        _logService.Warning("File Seervice: No worksheets found in {FileName}", file.FileName);

                        a2pOrder.ReadErrors.Add(new A2POrderError
                        {
                            Order = a2pOrder.Order,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.ReadService_WorksheetRead,
                            Description = $"No worksheets found in Order :{orderNumber} file {file.FileName}, file removed"
                        });

                        continue;
                    }
                }

                //get sales doc number and version if order exists
                //===================================================================
                (int, int) salesDoc = await _prefService.GetSalesDocAsync(orderNumber);
                a2pOrder.SalesDocNumber = salesDoc.Item1;
                a2pOrder.SalesDocVersion = salesDoc.Item2;

                if (a2pOrder.SalesDocNumber <= 0 || a2pOrder.SalesDocVersion <= 0)
                {
                    _logService.Warning("File Service : Order # {Order} not exists in PrefSuite.", orderNumber);
                    a2pOrder.ReadErrors.Add(new A2POrderError
                    {
                        Order = a2pOrder.Order,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.ReadService_OrderNotExistInPrefSuite,
                        Description = $"Order: {a2pOrder.Order} not exists in PrefSuite."
                    });
                }

                // Check if materials already exists
                //===================================================================


                string? itemsExistsInDB = await _prefService.MaterialsExistsAsync(orderNumber);
                if (!string.IsNullOrEmpty(itemsExistsInDB))
                {
                    _logService.Warning("File Service: Order {$Order} items already imported on {$Date}.", orderNumber, itemsExistsInDB);
                    if (DateTime.TryParse(itemsExistsInDB, out DateTime itemsExistsDate))
                    {
                        a2pOrder.OrderExists = itemsExistsDate;
                    }


                }
                string? materialsExistsInDB = await _prefService.MaterialsExistsAsync(orderNumber);
                if (!string.IsNullOrEmpty(materialsExistsInDB))
                {
                    _logService.Warning("File Service: Order {$Order} items already imported on {$Date}.", orderNumber, materialsExistsInDB);

                    if (DateTime.TryParse(materialsExistsInDB, out DateTime materialsExistsDate))
                    {
                        a2pOrder.OrderExists = materialsExistsDate;
                    }
                }

                if (!string.IsNullOrEmpty(a2pOrder.OrderExists.ToString()))
                {
                    a2pOrder.ReadErrors.Add(new A2POrderError
                    {
                        Order = a2pOrder.Order,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.SQLRepository_OrderAlreadyExistsInDB,
                        Description = $"Order with same number {orderNumber} already exists in SQL DB. Last import known date  {a2pOrder.OrderExists}"
                    });
                }


                _ = await A2POrderAgregator.AddOrUpdateOrderAsync(a2pOrderList, a2pOrder);

                progresValueCount += 1;
                _progressValue.Value = progresValueCount;
                progress?.Report(_progressValue);
                count++;

            }
            return a2pOrderList;
        }




        // Get files for a specific order asynchronously
        private async Task<List<A2PFile>> GetSingleOrderFilesAsync(string orderNumber, List<string> files, IProgress<ProgressValue>? progress = null)
        {
            List<string>? matchingFiles = files.Where(file => file.Contains(orderNumber)).ToList();
            List<A2PFile> a2pFileList = [];



            /*  -==Progress Update==- */
            {
                _progressValue.ProgressTask1 = $"Found {a2pFileList.Count} files.";
                progress?.Report(_progressValue);

            }
            _logService.Information("File Service: Order # {Order}. Found {FileCount} files.", orderNumber, files.Count);



            int count = 0;


            foreach (string file in matchingFiles)
            {
                string fileName = Path.GetFileName(file) ?? string.Empty;
                {
                    /*  -==Progress Update==- */
                    _progressValue.ProgressTask1 = $"Processing {Path.GetFileName(fileName)}. File {count + 1} of {matchingFiles.Count}.";
                    _progressValue.ProgressTask2 = $"Searching worksheets...";
                    _progressValue.ProgressTask3 = string.Empty;
                }
                A2PFile a2pFile = new()
                {

                    File = file.Trim(),
                    Order = orderNumber,
                    IsLocked = await IsLockedAsync(file.Trim()),
                    FilePath = Path.GetDirectoryName(file) ?? string.Empty,
                    FileName = fileName ?? string.Empty,
                };

                List<A2PFile> a2pFileListTemp = [a2pFile];
                List<A2PWorksheet> wr = await _readService.GetWorksheetListAsync(a2pFileListTemp, _progressValue, progress);
                a2pFile.Worksheets = wr;
                if (wr == null || !wr.Any())
                {
                    _logService.Warning("File Service: No worksheets found in {FileName}", fileName);
                    continue;
                }

                a2pFileList.Add(a2pFile);
                count++;
            }

            return a2pFileList;

        }

        public async Task<bool> IsLockedAsync(string filePath)
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



    }
}