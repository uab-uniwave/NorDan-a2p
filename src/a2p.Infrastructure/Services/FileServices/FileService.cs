// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;

using a2p.Application.Interfaces.Excel.Files;
using a2p.Application.Interfaces.Services;
using a2p.Application.Models;

using Microsoft.Extensions.Logging;

namespace a2p.Infrastructure.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly ISettingsService _settingsService;
        private readonly Application.Models.AppSettings _appSettings;
        private SettingsContainer _settingsContainer;

        public FileService(ISettingsService settingsService,
                           ILogger<FileService> logger)

        {
            _logger = logger;

            _settingsService = settingsService;


            _appSettings = _settingsService.LoadSettings();
            _settingsContainer = _settingsService.LoadAllSettings();
        }

        public List<string>? GetFiles()
        {

            List<string>? fileList;
            try
            {

                List<string> rawFileList = Directory.GetFiles(GetRootFolder()).ToList(); // Get all files in the root destinationFolder

                if (rawFileList == null || !rawFileList.Any())
                {
                    _logger.LogInformation("{$Class}.{$Method}.In folder: \"{$RootFolder}\" files not found.",
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

                    _logger.LogInformation("{$Class}.{$Method}.In folder: \"{$RootFolder}\" files not found.",
                     nameof(FileService),
                    nameof(GetFiles),
                    GetRootFolder());
                    return null;
                }
                else if (!fileList.Any())
                {
                    _logger.LogInformation("{$Class}.{$Method}.In folder: \"{$RootFolder}\" files not found.",
                       nameof(FileService),
                     nameof(GetFiles),
                    GetRootFolder());
                    return null;
                }
                else
                {
                    _logger.LogInformation("{$Class}.{$Method}. Found files ({!Count}) in folder \"{$RootFolder}\"",
                        nameof(FileService),
                    nameof(GetFiles),
                    fileList.Count,
                    GetRootFolder());

                    return fileList;
                }

            }

            catch (Exception ex)
            {
                _logger.LogError("{$Class}.{$Method}. Unhandled error getting files! Exception: {$Exception}",
                    nameof(FileService),
                    nameof(GetFiles),
                    ex.Message);
                return null;

            }
        }

        public List<Application.Models.File> GetOrderFiles(string order)
        {

            List<Application.Models.File> files = [];
            try
            {

                List<string> rawFileList = Directory.GetFiles(GetRootFolder()).ToList(); // Get all files in the root destinationFolder

                List<string> orderFiles = rawFileList.Where(f => f.StartsWith(order) && !f.Contains("~$") && f.EndsWith(".xlsx")).ToList(); // Get all files that match the order number

                for (int i = 0; i < orderFiles.Count; i++)
                {

                    Application.Models.File a2pFile = new()
                    {

                        FullName = orderFiles[i],
                        IsLocked = IsLocked(orderFiles[i]),
                        FilePath = Path.GetDirectoryName(orderFiles[i]) ?? string.Empty,
                        FileName = Path.GetFileName(orderFiles[i]) ?? string.Empty
                    };

                    files.Add(a2pFile);
                }
                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError("{$Class}.{$Method}. Unhandled error getting ordr files! Exception: {$Exception}",
                    nameof(FileService),
                    nameof(GetOrderFiles),
                    ex.Message);
                return files;
            }
        }

        public bool IsLocked(string filePath)
        {
            try

            {
                using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read);
                return false;
            }
            catch (Exception ex)
            {


                _logger.LogError("{$Class}.{$Method}. File \"{$File}\" is locked Exception: {$Exception}",
                   nameof(FileService),
                   nameof(IsLocked),
                   ex.Message);
                return true;
            }
        }

        //======================================================================
        // Write
        //======================================================================
        public void MoveOrderFiles(List<string> files, bool success)
        {


            try
            {

                foreach (string file in files)
                {

                    if (System.IO.File.Exists(file) && success == true)
                    {
                        string destinationFile = file.Replace(GetRootFolder(), GetSuccessFolder());
                        if (System.IO.File.Exists(destinationFile))
                        {
                            System.IO.File.Delete(destinationFile);
                        }


                        System.IO.File.Move(file, destinationFile);
                    }
                    else if (System.IO.File.Exists(file) && success == false)
                    {

                        string destinationFile = file.Replace(GetRootFolder(), GetFailedFolder());
                        if (System.IO.File.Exists(destinationFile))
                        {
                            System.IO.File.Delete(destinationFile);
                        }
                        System.IO.File.Move(file, destinationFile);

                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("{$Class}.{$Method}. Unhandled error moving files! Exception: {$Exception}",
                     nameof(FileService),
                     nameof(GetFiles),
                     ex.Message);

            }
        }

        public string GetRootFolder()
        {

            string folder = _appSettings.Folders.RootFolder ?? Path.Combine("C:", "Alu2Prefsuite");

            if (!Directory.Exists(folder))
            {
                _ = Directory.CreateDirectory(folder);
                _logger.LogInformation("{$Class}.{$Method}. Created folder for import files: \"{$Folder}\".",
                  nameof(FileService),
                  nameof(GetFiles),
                  folder);
            }

            return folder;

        }

        public string GetFailedFolder()
        {

            string folder = Path.Combine(GetRootFolder(), _appSettings.Folders.ImportFailed ?? "Failed");

            if (!Directory.Exists(folder))
            {
                _ = Directory.CreateDirectory(folder);
                _logger.LogInformation("{$Class}.{$Method}. Created folder for fail import files: \"{$Folder}\".",
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
                _logger.LogInformation("{$Class}.{$Method}. Created folder for success import files: \"{$Folder}\".",
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
                _logger.LogInformation("{$Class}.{$Method}. Created folder for log files: \"{$Folder}\".",
                     nameof(FileService),
                     nameof(GetFiles),
                     folder);

            }

            return folder;

        }



    }
}

