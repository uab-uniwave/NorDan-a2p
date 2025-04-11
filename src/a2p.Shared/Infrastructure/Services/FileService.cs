// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Models;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly ILogService _logService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly AppSettings _appSettings;
        private readonly SettingsContainer _settingsContainer;

        public FileService(IUserSettingsService userSettingsService,
                           ILogService logService)

        {
            _logService = logService;

            _userSettingsService = userSettingsService;
            _appSettings = _userSettingsService.LoadSettings();
            _settingsContainer = _userSettingsService.LoadAllSettings();
        }

        //======================================================================
        //🔵 Read
        //======================================================================
        //public async Task<ProgressValue> GetOrdersAsync()
        //{

        //    try
        //    {

        //       var result = await GetOrderNumbersAsync();

        //        // ======================================
        //        // 🔵 Iterate through orders
        //        // ======================================
        //        for (int i = 0; i < a2pOrders.Count; i++)
        //        {

        //            _progressValue.ProgressTask1 = $"Reading Order # {ordersCounter} of {a2pOrders.Count} - {a2pOrders[i].Order} ";
        //            _progressValue.Value++; // Increment the progress value - Read per Order 1
        //            _progress?.Report(_progressValue);

        //            a2pOrders[i].Files = await GetOrderFilesAsync(a2pOrders[i].Order, _progressValue, _progress);
        //            _progressValue.Value++; // Increment the progress value - Read per Order 2
        //            _progress?.Report(_progressValue);
        //            // Check if the order exists in the PrefSuite
        //            //===================================================================================================================================================

        //            int fileCounter = 0;

        //            // ======================================
        //            // 🔵 Iterate through files
        //            // ======================================

        //            for (int j = 0; j < a2pOrders[i].Files.Count; j++)
        //            {

        //                fileCounter++;

        //                _progressValue.ProgressTask2 = $"Reading File {fileCounter} of {a2pOrders[i].Files.Count()} - {a2pOrders[i].Files[j].FileName}";

        //                _progress?.Report(_progressValue);
        //                if (a2pOrders[i].Files[j].FileName.Contains(" "))
        //                {
        //                    a2pOrders[i].SourceAppType = SourceAppType.SapaV2;

        //                    if (a2pOrders[i].Files[j].FileName.Contains("Price_Details"))
        //                    {
        //                        a2pOrders[i].Files[j].IsOrderItemsFile = true;
        //                    }

        //                }
        //                if (a2pOrders[i].Files[j].IsLocked)
        //                {
        //                    _logService.Warning("File Service: File is locked {$FilePath}", a2pOrders[i].Files[j].FilePath);
        //                    a2pOrders[i].ReadErrors.Add(new A2PError
        //                    {
        //                        Order = a2pOrders[i].Order,
        //                        Level = ErrorLevel.Fatal,
        //                        Code = ErrorCode.FileSystemRead,
        //                        Message = $"Order {a2pOrders[i]} file {a2pOrders[i].Files[j].FileName} is locked by other application! "

        //                    });

        //                    a2pOrders[i].Files[j].Order = a2pOrders[i].Order;
        //                }

        //                List<A2PWorksheet> a2pWorksheets = await _excelReadService.GetWorksheetsAsync(a2pOrders[i].Files[j], _progressValue, _progress);

        //                a2pOrders[i].Files[j].Worksheets = a2pWorksheets;

        //                int worksheetCounter = 0;
        //                List<ItemDTO> resultItems;
        //                List<MaterialDTO> resultMateriales;
        //                for (int k = 0; k < a2pOrders[i].Files[j].Worksheets.Count; k++)
        //                {
        //                    worksheetCounter++;

        //                    a2pOrders[i].Files[j].Worksheets[k].Order = a2pOrders[i].Order;

        //                }

        //            }

        //            _progressValue.Value++; // Increment the progress value - Read per Order 3
        //            _progress?.Report(_progressValue);
        //        }
        //        return _progressValue;
        //    }
        //    catch (Exception ex)
        //    {

        //        _logService.Error("File Service: Unhandled Error.Exception{$Exception}", ex.Message);
        //        return _progressValue;
        //    }

        //}

        //private async Task<List<string>> GetOrderNumbersAsync()
        //{
        //    List<string> ordersList= new List<string>();

        //    try
        //    {
        //        List<A2POrder> orders = [];

        //        //GetrList of files in the root destinationFolder
        //        //==============================================================================================
        //        string rootFolder = _appSettings.Folders.RootFolder;
        //        List<string> files = (await Task.Run(() => Directory.GetFiles(rootFolder))).ToList(); // Get all files in the root destinationFolder
        //        if (files == null || !files.Any())
        //        {
        //            _logService.Information("File Service: No files found in {$RootFolder}", rootFolder);
        //            return ordersList;
        //        }

        //        //Extract order numbers from the file names and remove duplicates
        //        //==============================================================================================

        //        // if no orders found return empty list
        //        //============================================================================================== 
        //        if (orderNumbers == null || !orderNumbers.Any())
        //        {
        //            _logService.Information("File Service: No a2pOrders found in {$RootFolder}", rootFolder);

        //            return ordersList;
        //        }
        //        foreach (string orderNumber in orderNumbers)
        //        {

        //            A2POrder order = new() { Order = orderNumber };
        //            _dataCache.AddOrder(order);
        //            ordersList.Add(order.Order);
        //        }
        //        return ordersList;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.Error("File Service: Unhandled Error getting order numbers. Exception{$Exception}", ex.Message);
        //        return ordersList;
        //    }

        //}

        public List<string>? GetFiles()
        {

            List<string>? fileList;
            try
            {

                List<string> rawFileList = Directory.GetFiles(GetRootFolder()).ToList(); // Get all files in the root destinationFolder

                if (rawFileList == null || !rawFileList.Any())
                {
                    _logService.Information("{$Class}.{$Method}.In folder: \"{$RootFolder}\" files not found.",
                     nameof(FileService),
                    nameof(GetFiles),
                    GetRootFolder());
                    return null;
                }

                fileList = rawFileList
                    .Where(f => f != null && !f.Contains("~$") && f.EndsWith("xlsx"))
                    .OrderBy(file => file)
                    .ToList();
                if (fileList == null)
                {

                    _logService.Information("{$Class}.{$Method}.In folder: \"{$RootFolder}\" files not found.",
                     nameof(FileService),
                    nameof(GetFiles),
                    GetRootFolder());
                    return null;
                }
                else if (!fileList.Any())
                {
                    _logService.Information("{$Class}.{$Method}.In folder: \"{$RootFolder}\" files not found.",
                       nameof(FileService),
                     nameof(GetFiles),
                    GetRootFolder());
                    return null;
                }
                else
                {
                    _logService.Information("{$Class}.{$Method}. Found files ({!Count}) in folder \"{$RootFolder}\"",
                        nameof(FileService),
                    nameof(GetFiles),
                    fileList.Count,
                    GetRootFolder());

                    return fileList;
                }

            }

            catch (Exception ex)
            {
                _logService.Error("{$Class}.{$Method}. Unhandled error getting files! Exception: {$Exception}",
                    nameof(FileService),
                    nameof(GetFiles),
                    ex.Message);
                return null;

            }
        }

        public List<A2PFile> GetOrderFiles(string order)
        {

            List<A2PFile> a2pFiles = [];
            try
            {

                List<string> rawFileList = Directory.GetFiles(GetRootFolder()).ToList(); // Get all files in the root destinationFolder

                List<string> orderFiles = rawFileList.Where(f => f.StartsWith(order) && !f.Contains("~$") && f.EndsWith(".xlsx")).ToList(); // Get all files that match the order number

                for (int i = 0; i < orderFiles.Count; i++)
                {

                    A2PFile a2pFile = new()
                    {

                        File = orderFiles[i],
                        IsLocked = IsLocked(orderFiles[i]),
                        FilePath = Path.GetDirectoryName(orderFiles[i]) ?? string.Empty,
                        FileName = Path.GetFileName(orderFiles[i]) ?? string.Empty
                    };

                    a2pFiles.Add(a2pFile);
                }
                return a2pFiles;
            }
            catch (Exception ex)
            {
                _logService.Error("{$Class}.{$Method}. Unhandled error getting ordr files! Exception: {$Exception}",
                    nameof(FileService),
                    nameof(GetOrderFiles),
                    ex.Message);
                return a2pFiles;
            }
        }

        public bool IsLocked(string filePath)
        {
            try

            {
                using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return false;
            }
            catch (Exception ex)
            {
                _logService.Error("{$Class}.{$Method}. File \"{$File}\" is locked Exception: {$Exception}",
                   nameof(FileService),
                   nameof(IsLocked),
                   ex.Message);
                return false;
            }
            return true;
        }

        //======================================================================
        // Write
        //======================================================================
        public List<string>? MoveOrderFiles(List<string> files, bool success)
        {
            List<string> failedFiles = [];

            try
            {

                foreach (string file in files)
                {

                    if (File.Exists(file) && success)
                    {
                        File.Move(file, file.Replace(GetRootFolder(), GetSuccessFolder()));
                    }
                    else if (File.Exists(file) && success)
                    {
                        File.Move(file, file.Replace(GetRootFolder(), GetFailedFolder()));
                        failedFiles.Add(file.Replace(GetRootFolder(), GetFailedFolder()));

                    }
                }

                return failedFiles.Count > 0 ? failedFiles : null;
            }
            catch (Exception ex)
            {
                _logService.Error("{$Class}.{$Method}. Unhandled error moving files! Exception: {$Exception}",
                     nameof(FileService),
                     nameof(GetFiles),
                     ex.Message);
                return null;
            }
        }

        public string GetRootFolder()
        {

            string folder = _appSettings.Folders.RootFolder ?? Path.Combine("C:", "Alu2Prefsuite");

            if (!Directory.Exists(folder))
            {
                _ = Directory.CreateDirectory(folder);
                _logService.Information("{$Class}.{$Method}. Created folder for import files: \"{$Folder}\".",
                  nameof(FileService),
                  nameof(GetFiles),
                  folder);
            }

            return folder;

        }

        public string GetFailedFolder()
        {

            string folder = Path.Combine(GetRootFolder(), _appSettings.Folders.ImportSuccess ?? "Failed");

            if (!Directory.Exists(folder))
            {
                _ = Directory.CreateDirectory(folder);
                _logService.Information("{$Class}.{$Method}. Created folder for fail import files: \"{$Folder}\".",
                     nameof(FileService),
                     nameof(GetFiles),
                     folder);

            }

            return folder;
        }

        public string GetSuccessFolder()
        {
            string folder = Path.Combine(GetRootFolder(), _appSettings.Folders.ImportSuccess ?? "Success");

            if (!Directory.Exists(folder))
            {
                _ = Directory.CreateDirectory(folder);
                _logService.Information("{$Class}.{$Method}. Created folder for success import files: \"{$Folder}\".",
                     nameof(FileService),
                     nameof(GetFiles),
                     folder);

            }

            return folder;

        }
        public string GetLogFolder()
        {
            string folder = Path.Combine(GetRootFolder(), _appSettings.Folders.Log ?? "Log");

            if (!Directory.Exists(folder))
            {
                _ = Directory.CreateDirectory(folder);
                _logService.Information("{$Class}.{$Method}. Created folder for log files: \"{$Folder}\".",
                     nameof(FileService),
                     nameof(GetFiles),
                     folder);

            }

            return folder;

        }

    }
}
