// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Nodes;

using a2p.Shared.Application.Models;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Extensions.Configuration;

namespace a2p.Shared.Infrastructure.Services
{
    public class UserSettingsService : IUserSettingsService
    {

        private readonly string _appName = "Alu2PrefSuite";
        private readonly string _settingsFile;
        private readonly string _defaultSettingsFile;

        public UserSettingsService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string settingsFolder = Path.Combine(appDataPath, _appName);
            _settingsFile = Path.Combine(settingsFolder, "appsettings.json");
            _defaultSettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            if (!Directory.Exists(settingsFolder))
            {
                _ = Directory.CreateDirectory(settingsFolder);
            }

            if (!File.Exists(_settingsFile))
            {
                File.Copy(_defaultSettingsFile, _settingsFile);
            }

        }

        public void SaveSettings(AppSettings updatedAppSettings)
        {
            string jsonText = File.ReadAllText(_settingsFile);
            var fullJson = JsonNode.Parse(jsonText) as JsonObject;

            fullJson ??= [];

            fullJson["AppSettings"] = JsonSerializer.SerializeToNode(updatedAppSettings);

            File.WriteAllText(_settingsFile, fullJson.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }
        public void SaveConnectionString(string updatedConnectionString)
        {
            string jsonText = File.ReadAllText(_settingsFile);
            var fullJson = JsonNode.Parse(jsonText) as JsonObject;

            fullJson ??= [];

            if (fullJson["ConnectionStrings"] is not JsonObject connectionNode)
            {
                connectionNode = [];
            }

            connectionNode["DefaultConnection"] = updatedConnectionString;
            fullJson["ConnectionStrings"] = connectionNode;

            File.WriteAllText(_settingsFile, fullJson.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }

        public SettingsContainer LoadAllSettings()
        {
            var settings = new SettingsContainer();
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile(_settingsFile, optional: false, reloadOnChange: true)
                .Build();

            config.Bind(settings); // Binds both AppSettings and ConnectionStrings at root level
            return settings;
        }

        public AppSettings LoadSettings()
        {
            var settings = new AppSettings();
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile(_settingsFile, optional: false, reloadOnChange: false)
                .Build();

            config.GetSection("AppSettings").Bind(settings);
            return settings;
        }

        public string LoadSerilogMinimumLevel()
        {
            string jsonText = File.ReadAllText(_settingsFile);
            var json = JsonNode.Parse(jsonText);

            return json?["Serilog"]?["MinimumLevel"]?["Default"]?.ToString() ?? "Information";
        }

        public void SaveSerilogMinimumLevel(string level)
        {
            string jsonText = File.ReadAllText(_settingsFile);
            var json = JsonNode.Parse(jsonText) as JsonObject;

            json ??= [];

            if (json["Serilog"] is not JsonObject serilogNode)
            {
                serilogNode = [];
            }

            if (serilogNode["MinimumLevel"] is not JsonObject levelNode)
            {
                levelNode = [];
            }

            levelNode["Default"] = level;
            serilogNode["MinimumLevel"] = levelNode;
            json["Serilog"] = serilogNode;

            File.WriteAllText(_settingsFile, json.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }

        public string GetSettingsFilePath() => _settingsFile;
    }
}
