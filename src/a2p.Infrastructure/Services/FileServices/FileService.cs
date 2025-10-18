// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;

using a2p.Application.Interfaces.Files;
using a2p.Application.Interfaces.Services;
using a2p.Application.Models;

using Microsoft.Extensions.Logging;

namespace a2p.Infrastructure.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly ISettingsService _settingsService;
        private readonly AppSettings _appSettings;
        private SettingsContainer _settingsContainer;

        public FileService(ISettingsService settingsService,
                           ILogger<FileService> logger)

        {
            _logger = logger;
            _settingsService = settingsService;
            _appSettings = _settingsService.GetAppSettings();
            _settingsContainer = _settingsService.GetSettings();
        }

        public List<string> GetFiles()
        {
            try
            {

                List<string> _files = Directory.GetFiles(GetRootFolder()).ToList() ?? []; // Get all files in the root destinationFolder

                return _files
                    .Where(f => f != null && !f.Contains("~$") && f.EndsWith("xlsx"))
                    .OrderBy(file => file)
                    .ToList() ?? [];


            }

            catch (Exception ex)
            {
                _logger.LogError("{$Class}.{$Method}. Unhandled error getting files! Exception: {$Exception}",
                    nameof(FileService),
                    nameof(GetFiles),
                    ex.Message);
                return [];

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
                   filePath,
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

