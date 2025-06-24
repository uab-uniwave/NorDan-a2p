// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Models;
using a2p.Shared.Infrastructure.Interfaces;

using System.Data;

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
                using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read);
                return false;
            }
            catch (Exception ex)
            {


                _logService.Error("{$Class}.{$Method}. File \"{$File}\" is locked Exception: {$Exception}",
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

                    if (File.Exists(file) && success == true)
                    {
                        File.Move(file, file.Replace(GetRootFolder(), GetSuccessFolder()));
                    }
                    else if (File.Exists(file) && success == false)
                    {

                        File.Move(file, file.Replace(GetRootFolder(), GetFailedFolder()));

                    }
                }


            }
            catch (Exception ex)
            {
                _logService.Error("{$Class}.{$Method}. Unhandled error moving files! Exception: {$Exception}",
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
                _logService.Information("{$Class}.{$Method}. Created folder for import files: \"{$Folder}\".",
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

