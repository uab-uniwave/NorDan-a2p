using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using a2p.Shared.Application.Models;
using a2p.Shared.Infrastructure.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a2p.WinForm.ChildForms.Tests
{
    [TestClass]
    public class SettingFormTests
    {
        private Mock<ILogService> _mockLogService;
        private Mock<IUserSettingsService> _mockUserSettingsService;
        private SettingForm _settingForm;
        private AppSettings _appSettings;

        [TestInitialize]
        public void Setup()
        {
            _mockLogService = new Mock<ILogService>();
            _mockUserSettingsService = new Mock<IUserSettingsService>();
            _appSettings = new AppSettings
            {
                Folders = new FolderSettings
                {
                    RootFolder = "C:\\RootFolder",
                    ImportSuccess = "C:\\ImportSuccess",
                    ImportFailed = "C:\\ImportFailed",
                    Log = "C:\\Log"
                },
                LogLevelFilter = new List<string> { "Information" }
            };

            _mockUserSettingsService.Setup(s => s.LoadSettings()).Returns(_appSettings);
            _settingForm = new SettingForm(_mockLogService.Object, _mockUserSettingsService.Object);
        }

        [TestMethod]
        public async Task btnSave_Click_AllFoldersExist_SavesSettings()
        {
            // Arrange
            Directory.CreateDirectory(_appSettings.Folders.RootFolder);
            Directory.CreateDirectory(_appSettings.Folders.ImportSuccess);
            Directory.CreateDirectory(_appSettings.Folders.ImportFailed);
            Directory.CreateDirectory(_appSettings.Folders.Log);

            // Act
            _settingForm.btnSave_Click(null, EventArgs.Empty);

            // Assert
            _mockUserSettingsService.Verify(s => s.SaveSettings(_appSettings), Times.Once);
            _mockUserSettingsService.Verify(s => s.SaveConnectionString(It.IsAny<string>()), Times.Once);
            _mockUserSettingsService.Verify(s => s.SaveSerilogMinimumLevel(It.IsAny<string>()), Times.Once);
            _mockLogService.Verify(l => l.Information("Settings saved successfully."), Times.Once);
        }

        [TestMethod]
        public async Task btnSave_Click_FoldersDoNotExist_UserChoosesToCreate_SavesSettings()
        {
            // Arrange
            Directory.Delete(_appSettings.Folders.RootFolder, true);
            Directory.Delete(_appSettings.Folders.ImportSuccess, true);
            Directory.Delete(_appSettings.Folders.ImportFailed, true);
            Directory.Delete(_appSettings.Folders.Log, true);

            // Simulate user choosing to create folders
            _settingForm.MessageBoxOverride = (text, caption, buttons, icon) => DialogResult.Yes;

            // Act
            _settingForm.btnSave_Click(null, EventArgs.Empty);

            // Assert
            _mockUserSettingsService.Verify(s => s.SaveSettings(_appSettings), Times.Once);
            _mockUserSettingsService.Verify(s => s.SaveConnectionString(It.IsAny<string>()), Times.Once);
            _mockUserSettingsService.Verify(s => s.SaveSerilogMinimumLevel(It.IsAny<string>()), Times.Once);
            _mockLogService.Verify(l => l.Information("Settings saved successfully."), Times.Once);
        }

        [TestMethod]
        public async Task btnSave_Click_FoldersDoNotExist_UserChoosesNotToCreate_DoesNotSaveSettings()
        {
            // Arrange
            Directory.Delete(_appSettings.Folders.RootFolder, true);
            Directory.Delete(_appSettings.Folders.ImportSuccess, true);
            Directory.Delete(_appSettings.Folders.ImportFailed, true);
            Directory.Delete(_appSettings.Folders.Log, true);

            // Simulate user choosing not to create folders
            _settingForm.MessageBoxOverride = (text, caption, buttons, icon) => DialogResult.No;

            // Act
            _settingForm.btnSave_Click(null, EventArgs.Empty);

            // Assert
            _mockUserSettingsService.Verify(s => s.SaveSettings(It.IsAny<AppSettings>()), Times.Never);
            _mockUserSettingsService.Verify(s => s.SaveConnectionString(It.IsAny<string>()), Times.Never);
            _mockUserSettingsService.Verify(s => s.SaveSerilogMinimumLevel(It.IsAny<string>()), Times.Never);
            _mockLogService.Verify(l => l.Information(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task btnSave_Click_ExceptionThrown_ShowsErrorMessage()
        {
            // Arrange
            _mockUserSettingsService.Setup(s => s.SaveSettings(It.IsAny<AppSettings>())).Throws(new Exception("Test exception"));

            // Act
            _settingForm.btnSave_Click(null, EventArgs.Empty);

            // Assert
            _mockLogService.Verify(l => l.Error(It.IsAny<Exception>(), "Failed to save settings."), Times.Once);
            _mockUserSettingsService.Verify(s => s.SaveSettings(It.IsAny<AppSettings>()), Times.Once);
        }
    }
}
