// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Nodes;

using a2p.Application.Interfaces.Services;
using a2p.Application.Models;

using Microsoft.Extensions.Configuration;

namespace a2p.Infrastructure.Services.SettingsService
{
    public class SettingsService : ISettingsService
    {

        private readonly string _appName = "Alu2PrefSuite";
        private readonly string _settingsFile;
        private readonly string _defaultSettingsFile;

        public SettingsService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string settingsFolder = Path.Combine(appDataPath, _appName);
            _settingsFile = Path.Combine(settingsFolder, "appsettings.json");
            _defaultSettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            if (!Directory.Exists(settingsFolder))
            {
                _ = Directory.CreateDirectory(settingsFolder);
            }

            if (!System.IO.File.Exists(_settingsFile))
            {
                System.IO.File.Copy(_defaultSettingsFile, _settingsFile);
            }

        }
        public string GetSettingsFilePath() => _settingsFile;
        public AppSettings GetAppSettings()
        {
            AppSettings settings = new();
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile(_settingsFile, optional: false, reloadOnChange: false)
                .Build();

            config.GetSection("AppSettings").Bind(settings);
            return settings;
        }
        public string GetConnectionString()
        {
            string jsonText = System.IO.File.ReadAllText(_settingsFile);
            JsonNode? json = JsonNode.Parse(jsonText);

            return json?["ConnectionStrings"]?["DefaultConnection"]?.ToString() ?? string.Empty;
        }

        public string GetSerilogLevel()
        {
            string jsonText = System.IO.File.ReadAllText(_settingsFile);
            JsonNode? json = JsonNode.Parse(jsonText);

            return json?["Serilog"]?["MinimumLevel"]?["Default"]?.ToString() ?? "Information";
        }

        public void SetAppSettings(AppSettings updatedAppSettings)
        {
            string jsonText = System.IO.File.ReadAllText(_settingsFile);
            JsonObject? fullJson = JsonNode.Parse(jsonText) as JsonObject;

            fullJson ??= [];

            fullJson["AppSettings"] = JsonSerializer.SerializeToNode(updatedAppSettings);

            System.IO.File.WriteAllText(_settingsFile, fullJson.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }
        public void SetConnectionString(string updatedConnectionString)
        {
            string jsonText = System.IO.File.ReadAllText(_settingsFile);
            JsonObject? fullJson = JsonNode.Parse(jsonText) as JsonObject;

            fullJson ??= [];

            if (fullJson["ConnectionStrings"] is not JsonObject connectionNode)
            {
                connectionNode = [];
            }

            connectionNode["DefaultConnection"] = updatedConnectionString;
            fullJson["ConnectionStrings"] = connectionNode;

            System.IO.File.WriteAllText(_settingsFile, fullJson.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }

        public Application.Models.SettingsContainer GetSettings()
        {
            SettingsContainer settings = new();
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile(_settingsFile, optional: false, reloadOnChange: true)
                .Build();

            config.Bind(settings); // Binds both AppSettings and ConnectionStrings at root level
            return settings;
        }





        public void SetSerilogLevel(string level)
        {
            string jsonText = System.IO.File.ReadAllText(_settingsFile);
            JsonObject? json = JsonNode.Parse(jsonText) as JsonObject;

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

            System.IO.File.WriteAllText(_settingsFile, json.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }

    }
}
