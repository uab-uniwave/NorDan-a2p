using a2p.Shared.Core.Utils;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace a2p.WinForm.ChildForms
{
    public partial class SettingForm : Form
    {
        private readonly IConfiguration _configuration;
        private readonly ILogService _logger;
        private bool _isSettingsChanged = false; // Flag to track changes

        public SettingForm(IConfiguration configuration, ILogService logger)
        {
            _configuration=configuration;
            _logger=logger;

            this.SuspendLayout();
            this.AutoScaleMode=AutoScaleMode.Dpi;
            InitializeComponent();
            this.PerformAutoScale(); // Ensure everything is scaled correctly (optional)
            this.ResumeLayout(true); // 


            // Add change detection handlers for controls
            tbWorkingFolder.TextChanged+=OnSettingChanged;
            tbSuccess.TextChanged+=OnSettingChanged;
            tbFailed.TextChanged+=OnSettingChanged;
            chbStaging.CheckedChanged+=OnSettingChanged;
            chbLoadOnStart.CheckedChanged+=OnSettingChanged;
        }

        private void OnSettingChanged(object? sender, EventArgs e)
        {
            _isSettingsChanged=true;
            btSave.Enabled=true; // Enable the Save button when a change is detected
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            // Load initial settings into controls
            tbWorkingFolder.Text=_configuration["AppSettings:WorkingFolder"]??@"C:\Temp\Import";
            tbSuccess.Text=_configuration["AppSettings:SuccessFolder"]??@"C:\Temp\Import\SC\";
            tbFailed.Text=_configuration["AppSettings:UnsuccessFolder"]??@"C:\Temp\Import\US\";
            chbStaging.Checked=bool.Parse(_configuration["AppSettings:Staging"]??"false");
            chbLoadOnStart.Checked=bool.Parse(_configuration["AppSettings:RefreshFilesOnStartup"]??"false");

            // Initially disable the Save button
            btSave.Enabled=false;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Save the updated settings
                _configuration["AppSettings:WorkingFolder"]=tbWorkingFolder.Text;
                _configuration["AppSettings:SuccessFolder"]=tbSuccess.Text;
                _configuration["AppSettings:UnsuccessFolder"]=tbFailed.Text;
                _configuration["AppSettings:Staging"]=chbStaging.Checked.ToString();
                _configuration["AppSettings:RefreshFilesOnStartup"]=chbLoadOnStart.Checked.ToString();

                // Save the configuration to appsettings.json
                IConfigurationRoot configurationRoot = (IConfigurationRoot)_configuration;
                if (configurationRoot.Providers.FirstOrDefault(p => p is JsonConfigurationProvider) is JsonConfigurationProvider fileProvider)
                {
                    JsonConfigurationSource jsonConfigSource = (JsonConfigurationSource)fileProvider.Source;
                    string? filePath = jsonConfigSource.Path;
                    IConfigurationRoot jsonConfig = new ConfigurationBuilder().AddJsonFile(filePath).Build();
                    jsonConfig["AppSettings:WorkingFolder"]=tbWorkingFolder.Text;
                    jsonConfig["AppSettings:SuccessFolder"]=tbSuccess.Text;
                    jsonConfig["AppSettings:UnsuccessFolder"]=tbFailed.Text;
                    jsonConfig["AppSettings:Staging"]=chbStaging.Checked.ToString();
                    jsonConfig["AppSettings:RefreshFilesOnStartup"]=chbLoadOnStart.Checked.ToString();
                    File.WriteAllText(filePath, jsonConfig.GetDebugView());
                }

                _logger.Information("Settings have been successfully saved.");

                // Reset the change tracking flag and disable the Save button
                _isSettingsChanged=false;
                btSave.Enabled=false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while saving settings.");
            }
        }

        private void SettingForm_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            this.PerformAutoScale();

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}