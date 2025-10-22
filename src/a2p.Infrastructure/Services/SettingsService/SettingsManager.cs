using Application.Interfaces.Services;
using Application.Models;
namespace Infrastructure.Services.SettingsService
{
    public class SettingsManager
    {
        private readonly ISettingsService _SettingsService;

        public SettingsManager(ISettingsService settingsService) => _SettingsService = settingsService;

        public AppSettings LoadSettings() => _SettingsService.GetAppSettings();

        public void SaveSettings(AppSettings settings) => _SettingsService.SetAppSettings(settings);
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

                if (FileDto.Exists(logFile))
                {
                    fileExists = true;
                }


                if (fileExists)
                {
                    FileDto.Delete(logFile);
                }
            }

            catch (IOException ex)
            {
                Debug.WriteLine($"PR. Deleting file: {logFile} failed. Exception: {ex.Message}");
                throw;
            }
*/
