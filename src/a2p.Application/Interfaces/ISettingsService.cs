using a2p.Application.Models;

namespace a2p.Application.Interfaces
{
    public interface ISettingsService
    {
        SettingsContainer LoadAllSettings();
        AppSettings LoadSettings();
        void SaveSettings(AppSettings updatedAppSettings);
        void SaveConnectionString(string updatedConnectionString);
        string GetSettingsFilePath();
        void SaveSerilogMinimumLevel(string level);
        string LoadSerilogMinimumLevel();
    }
}
