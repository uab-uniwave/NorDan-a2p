using a2p.Application.Interfaces.Services;
using a2p.Application.Models;
namespace a2p.Infrastructure.Services.SettingsService
{
    public class SettingsManager
    {
        private readonly ISettingsService _SettingsService;

        public SettingsManager(ISettingsService settingsService) => _SettingsService = settingsService;

        public AppSettings LoadSettings() => _SettingsService.LoadSettings();

        public void SaveSettings(AppSettings settings) => _SettingsService.SaveSettings(settings);
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
