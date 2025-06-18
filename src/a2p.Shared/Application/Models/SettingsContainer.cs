// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace a2p.Shared.Application.Models
{
    public class SettingsContainer
    {
        public AppSettings AppSettings { get; set; } = new();
        public Dictionary<string, string> ConnectionStrings { get; set; } = [];
        public SerilogSettings Serilog { get; set; } = new();
    }

    public class AppSettings
    {

        public FolderSettings Folders { get; set; } = new();

        public ModelSettings Model { get; set; } = new();

        public bool RefreshFilesOnStartup { get; set; } = false;
        public bool Staging { get; set; } = false;
        public List<string> LogLevelFilter { get; set; } = ["Information"];
             
    }

    public class FolderSettings
    {
        public string RootFolder { get; set; } = @"C:\\Temp\\Import2";
        public string ImportSuccess { get; set; } = "Import_Success";
        public string ImportFailed { get; set; } = "Import_Failed";
        public string Log { get; set; } = "Log";
    }

    public class ModelSettings
    {

        public string Sapa { get; set; } = "Sapa_ALU";
        public string TechnoDesign { get; set; } = "Sapa_ALU";
        public string Schuco { get; set; } = "Schuco_ALU";
    }

    public class ConnectionStrings
    {
        public string? DefaultConnection { get; set; } = string.Empty;
    }

    public class SerilogSettings
    {
        public MinimumLevel MinimumLevel { get; set; } = new();
    }

    public class MinimumLevel
    {
        public string Default { get; set; } = "Information";
    }
}
