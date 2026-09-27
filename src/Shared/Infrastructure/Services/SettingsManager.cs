// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Models;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Infrastructure.Services
{
    public class SettingsManager
    {
        private readonly IUserSettingsService _userSettingsService;

        public SettingsManager(IUserSettingsService userSettingsService) => _userSettingsService = userSettingsService;

        public AppSettings LoadSettings() => _userSettingsService.LoadSettings();

        public void SaveSettings(AppSettings settings) => _userSettingsService.SaveSettings(settings);
    }
}

/*string rootFolder = configuration["AppSettings:Folders:Root"] ?? @"C:\\Temp\\Import";
EnsureDirectoryExist(rootFolder, "Root", configuration);
string importSuccessFolder = Path.Combine(rootFolder, configuration["AppSettings:Folders:ImportSuccess"] ?? "Import_Success");
EnsureDirectoryExist(importSuccessFolder, "ImportSuccess", configuration);
string importFailedFolder = Path.Combine(rootFolder, configuration["AppSettings:Folders:ImportFailed"] ?? "Import_Failed");
EnsureDirectoryExist(importFailedFolder, "ImportFailed", configuration);
string logFolder = Path.Combine(rootFolder, configuration["AppSettings:Folders:Log"] ?? "Log");
EnsureDirectoryExist(logFolder, "Log", configuration);
string logFile = Path.Combine(logFolder, "a2pLog.json");





           

            try
            {

                bool fileExists = false;

                if (File.Exists(logFile))
                {
                    fileExists = true;
                }


                if (fileExists)
                {
                    File.Delete(logFile);
                }
            }

            catch (IOException ex)
            {
                Debug.WriteLine($"PR. Deleting file: {logFile} failed. Exception: {ex.Message}");
                throw;
            }
*/
