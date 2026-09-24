// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Models;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System.Text.Json;

namespace a2p.WinForm.ChildForms
{
    public partial class SettingForm : Form
    {
        private readonly ILogService _logService;
        private readonly IUserSettingsService _userSettingsService;
        private bool _isSettingsChanged = false; // Flag to track changes
        private AppSettings _currentSettings; // Store the current settings
        private readonly IConfiguration _configuration;

        public SettingForm(ILogService logService, IUserSettingsService userSettingsService)
        {
            _logService = logService;
            _userSettingsService = userSettingsService;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddUserSecrets<SettingForm>();
            _configuration = builder.Build();

            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.SuspendLayout();
            InitializeComponent();

            // Add change detection handlers for controls
            txbWorkingFolder.TextChanged += OnSettingChanged;
            txbFailedFolder.TextChanged += OnSettingChanged;
            txbSuccessFolder.TextChanged += OnSettingChanged;
            txbLogFolder.TextChanged += OnSettingChanged;
            txbServerName.TextChanged += OnSettingChanged;
            txbDatabaseName.TextChanged += OnSettingChanged;
            txbUserName.TextChanged += OnSettingChanged;
            txbPassword.TextChanged += OnSettingChanged;
            txbServerName.TextChanged += OnSettingChanged;
            chxStaging.CheckedChanged += OnSettingChanged;
            chxLoadOnStart.CheckedChanged += OnSettingChanged;
            chxTrusted.CheckedChanged += OnSettingChanged;
            cbxLogLevel.SelectedIndexChanged += OnSettingChanged;
            _currentSettings = _userSettingsService.LoadSettings();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            AutoScaleMode = AutoScaleMode.Dpi;
            this.PerformAutoScale();
            ApplySettingsToForm(_currentSettings);
            btnSave.Enabled = false;

            // Call RegisterChangeDetection to set up additional change detection
            RegisterChangeDetection();
        }

        private void SettingForm_Shown(object sender, EventArgs e)
        {
            ApplySettingsToForm(_currentSettings);
            if (chxTrusted.Checked)
            {
                txbUserName.Enabled = false;
                txbPassword.Enabled = false;
            }
            else
            {
                txbUserName.Enabled = true;
                txbPassword.Enabled = true;
            }

            PerformLayout();
            tlpSettings.PerformLayout();
        }

        private void SettingForm_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            this.PerformAutoScale();
            tlpSettings.PerformLayout();
        }

        private void btnWorkingFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog workingFolder = new()
            {
                Description = "Select the working folder",
                SelectedPath = txbWorkingFolder.Text
            };
            if (workingFolder.ShowDialog() == DialogResult.OK)
            {
                txbWorkingFolder.Text = workingFolder.SelectedPath;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string connectionString = BuildConnectionStringFromForm();
            _ = TestSqlConnection(connectionString, out string error)
                ? MessageBox.Show("Connection successful!")
                : MessageBox.Show($"Connection failed: {error}");
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateSettingsFromForm(); // populate currentSettings from form

                // Check if folders exist
                string[] folders = {
                        Path.Combine(_currentSettings.Folders.RootFolder, _currentSettings.Folders.ImportSuccess),
                        Path.Combine(_currentSettings.Folders.RootFolder, _currentSettings.Folders.ImportFailed),
                        Path.Combine(_currentSettings.Folders.RootFolder, _currentSettings.Folders.Log),
                    };

                bool allFoldersExist = folders.All(Directory.Exists);

                if (!allFoldersExist)
                {
                    DialogResult result = MessageBox.Show(
                        "One or more folders do not exist. Do you want to create them?",
                        "Create Folders",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.No)
                    {
                        _ = MessageBox.Show("Save operation cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Create missing folders
                    foreach (string folder in folders)
                    {
                        if (!Directory.Exists(folder))
                        {
                            _ = Directory.CreateDirectory(folder);
                        }
                    }
                }

                // Save settings asynchronously
                await Task.Run(() => _userSettingsService.SaveSettings(_currentSettings));
                await Task.Run(() => _userSettingsService.SaveConnectionString(BuildConnectionStringFromForm()));

                // Update UI controls on the UI thread
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        _isSettingsChanged = false;
                        btnSave.Enabled = false;
                        _logService.Information("Settings saved successfully.");
                        _ = MessageBox.Show("Settings saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
                else
                {
                    _isSettingsChanged = false;
                    btnSave.Enabled = false;
                    _logService.Information("Settings saved successfully.");
                    _ = MessageBox.Show("Settings saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        _logService.Error("Failed to save settings.");
                        _ = MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    _logService.Error("Failed to save settings.");
                    _ = MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void chxTrusted_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chxTrusted.Checked;
            txbUserName.Enabled = !isChecked;
            txbPassword.Enabled = !isChecked;
        }

        private void ApplySettingsToForm(AppSettings settings)
        {
            txbWorkingFolder.Text = settings.Folders.RootFolder;
            txbSuccessFolder.Text = string.IsNullOrEmpty(settings.Folders.ImportSuccess.ToString()) == true
                ? "Import_Success" : settings.Folders.ImportSuccess.ToString();
            txbFailedFolder.Text = string.IsNullOrEmpty(settings.Folders.ImportFailed.ToString()) == true
               ? "Import_Failed" : settings.Folders.ImportFailed.ToString();

            txbLogFolder.Text = string.IsNullOrEmpty(settings.Folders.Log.ToString()) == true
               ? "Log" : settings.Folders.Log.ToString();

            cbxLogLevel.SelectedItem = _userSettingsService.LoadSerilogMinimumLevel();
            chxTrusted.Checked = IsIntegratedSecurityEnabled();
            txbServerName.Text = ExtractValueFromConnectionString("Data Source");
            txbDatabaseName.Text = ExtractValueFromConnectionString("Initial Catalog");
            txbUserName.Text = ExtractValueFromConnectionString("User");
            txbPassword.Text = ExtractValueFromConnectionString("Password");
            chxStaging.Checked = settings.Staging;
            chxStaging.Checked = settings.Staging;
            chxLoadOnStart.Checked = settings.RefreshFilesOnStartup;
            cbxLogLevel.SelectedItem = settings.LogLevelFilter.FirstOrDefault(); // or another logic
        }

        private bool IsIntegratedSecurityEnabled()
        {
            string value = ExtractValueFromConnectionString("Integrated Security");
            return value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        private bool TestSqlConnection(string connectionString, out string error)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                error = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private void UpdateSettingsFromForm()
        {
            _currentSettings.Folders.RootFolder = txbWorkingFolder.Text;
            _currentSettings.Folders.ImportSuccess = txbSuccessFolder.Text;
            _currentSettings.Folders.ImportFailed = txbFailedFolder.Text;
            _currentSettings.Folders.Log = txbLogFolder.Text;
            _currentSettings.Staging = chxStaging.Checked;
            _currentSettings.RefreshFilesOnStartup = chxLoadOnStart.Checked;

            _currentSettings.LogLevelFilter = [cbxLogLevel.SelectedItem?.ToString() ?? "Information"];
        }

        private string BuildConnectionStringFromForm()
        {
            try
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = txbServerName.Text,
                    InitialCatalog = txbDatabaseName.Text,
                    IntegratedSecurity = chxTrusted.Checked
                };

                if (chxTrusted.Checked == false)
                {
                    // Retrieve username and password from UserSecrets
                    string username = _configuration["DbUsername"] ?? string.Empty;
                    string password = _configuration["DbPassword"] ?? string.Empty;

                    builder.UserID = username;
                    builder.Password = password;
                }

                builder.TrustServerCertificate = true; // Optional, based on your defaults
                builder.Encrypt = false;
                builder.ConnectTimeout = 30;
                return builder.ToString();
            }
            catch
            {
                _logService.Error("Check Settings. User name password, SQL Server and DB!");
                return "";
            }
        }

        public string ExtractValueFromConnectionString(string key)
        {
            string settingsJson = File.ReadAllText(_userSettingsService.GetSettingsFilePath());
            var jsonDoc = JsonDocument.Parse(settingsJson);

            if (!jsonDoc.RootElement.TryGetProperty("ConnectionStrings", out JsonElement connectionStrings))
            {
                return string.Empty;
            }

            if (!connectionStrings.TryGetProperty("DefaultConnection", out JsonElement connectionValue))
            {
                return string.Empty;
            }

            string[]? parts = connectionValue.GetString()?.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (parts == null)
            {
                return string.Empty;
            }

            foreach (string part in parts)
            {
                if (part.Trim().StartsWith(key, StringComparison.OrdinalIgnoreCase))
                {
                    string[] kv = part.Split('=', 2);
                    if (kv.Length == 2)
                    {
                        return kv[1].Trim();
                    }
                }
            }

            return string.Empty;
        }

        private void RegisterChangeDetection()
        {
            foreach (Control control in this.Controls.OfType<Control>())
            {
                if (control is TextBox textBox)
                {
                    textBox.TextChanged += OnSettingChanged;
                }

                if (control is CheckBox checkBox)
                {
                    checkBox.CheckedChanged += OnSettingChanged;
                }

                if (control is ComboBox comboBox)
                {
                    comboBox.SelectedIndexChanged += OnSettingChanged;
                }
            }
        }

        private void OnSettingChanged(object? sender, EventArgs e)
        {
            _isSettingsChanged = true;
            btnSave.Enabled = true;
            UpdateSettingsFromForm();
        }
    }
}

