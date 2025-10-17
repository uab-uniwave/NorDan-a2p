using a2p.Application.Models;

namespace a2p.Application.Interfaces.Services
{
    public interface ISettingsService
    {
        SettingsContainer GetSettings();
        AppSettings GetAppSettings();
        void SetAppSettings(AppSettings updatedAppSettings);

        string GetConnectionString();

        void SetConnectionString(string updatedConnectionString);
        string GetSettingsFilePath();
        void SetSerilogLevel(string level);
        string GetSerilogLevel();
    }
}
