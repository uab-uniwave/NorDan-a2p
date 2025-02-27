// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace a2p.WinForm.ChildForms
{
    public partial class SettingForm : Form
    {
        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;
        private bool _isSettingsChanged = false; // Flag to track changes

        public SettingForm(IConfiguration configuration, ILogService logService)
        {
            _configuration = configuration;
            _logService = logService;

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.SuspendLayout();
            InitializeComponent();

            // Add change detection handlers for controls
            tbxWorkingFolder.TextChanged += OnSettingChanged;
            tbxSuccess.TextChanged += OnSettingChanged;
            tbxFailed.TextChanged += OnSettingChanged;
            cbxStaging.CheckedChanged += OnSettingChanged;
            cbxLoadOnStart.CheckedChanged += OnSettingChanged;
        }

        private void OnSettingChanged(object? sender, EventArgs e)
        {
            _isSettingsChanged = true;
            btnSave.Enabled = true; // Enable the Save button when a change is detected
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {

            AutoScaleMode = AutoScaleMode.Dpi;
            // Load initial settings into controls
            tbxWorkingFolder.Text = _configuration["AppSettings:WorkingFolder"] ?? @"C:\Temp\Import";
            tbxSuccess.Text = _configuration["AppSettings:SuccessFolder"] ?? @"C:\Temp\Import\Import_Success";
            tbxFailed.Text = _configuration["AppSettings:UnsuccessFolder"] ?? @"C:\Temp\Import\Import_Failed";
            cbxStaging.Checked = bool.Parse(_configuration["AppSettings:Staging"] ?? "false");
            cbxLoadOnStart.Checked = bool.Parse(_configuration["AppSettings:RefreshFilesOnStartup"] ?? "false");

            // Initially disable the Save button
            btnSave.Enabled = false;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Save the updated settings
                _configuration["AppSettings:WorkingFolder"] = tbxWorkingFolder.Text;
                _configuration["AppSettings:SuccessFolder"] = tbxSuccess.Text;
                _configuration["AppSettings:UnsuccessFolder"] = tbxFailed.Text;
                _configuration["AppSettings:Staging"] = cbxStaging.Checked.ToString();
                _configuration["AppSettings:RefreshFilesOnStartup"] = cbxLoadOnStart.Checked.ToString();

#pragma warning disable CA1416 // Validate platform compatibility
                // Save the configuration to appsettings.json
                IConfigurationRoot configurationRoot = (IConfigurationRoot)_configuration;
                if (configurationRoot.Providers.FirstOrDefault(p => p is JsonConfigurationProvider) is JsonConfigurationProvider fileProvider)
                {
                    JsonConfigurationSource jsonConfigSource = (JsonConfigurationSource)fileProvider.Source;
                    string? filePath = jsonConfigSource.Path;
                    IConfigurationRoot jsonConfig = new ConfigurationBuilder().AddJsonFile(filePath ?? string.Empty).Build();

                    jsonConfig["AppSettings:WorkingFolder"] = tbxWorkingFolder.Text.ToString();

                    jsonConfig["AppSettings:SuccessFolder"] = tbxSuccess.Text.ToString();
                    jsonConfig["AppSettings:UnsuccessFolder"] = tbxFailed.Text.ToString();
                    jsonConfig["AppSettings:Staging"] = cbxStaging.Checked.ToString().ToString();
                    jsonConfig["AppSettings:RefreshFilesOnStartup"] = cbxLoadOnStart.Checked.ToString();
                    File.WriteAllText(filePath ?? string.Empty, jsonConfig.GetDebugView());
                }
#pragma warning restore CA1416 // Validate platform compatibility
                _logService.Information("Settings have been successfully saved.");

                // Reset the change tracking flag and disable the Save button
                _isSettingsChanged = false;
                btnSave.Enabled = false;
            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error while saving settings.");
            }
        }

        private void SettingForm_DpiChanged(object sender, DpiChangedEventArgs e) => this.PerformAutoScale();

        private void btnWorkingFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog workingFolder = new()
            {
                Description = "Select the working folder",
                SelectedPath = tbxWorkingFolder.Text
            };
            if (workingFolder.ShowDialog() == DialogResult.OK)
            {
                tbxWorkingFolder.Text = workingFolder.SelectedPath;
            }

        }

        private void SettingForm_Shown(object sender, EventArgs e) => PerformLayout();

        private void btnSuccess_Click(object sender, EventArgs e)
        {
#if WINDOWS
            FolderBrowserDialog successFolder = new()
            {
                Description = "Select folder for failed processed files.",
                SelectedPath = tbxSuccess.Text
            };
            if (successFolder.ShowDialog() == DialogResult.OK)
            {
                tbxFailed.Text = successFolder.SelectedPath;
            }
#endif
        }

        private void btnFailed_Click(object sender, EventArgs e)
        {
#if WINDOWS
            FolderBrowserDialog failedFolder = new()
            {
                Description = "Select folder for failed processed files.",
                SelectedPath = tbxFailed.Text
            };

            if (failedFolder.ShowDialog() == DialogResult.OK)
            {
                tbxFailed.Text = failedFolder.SelectedPath;
            }
#endif
        }
    }
}
