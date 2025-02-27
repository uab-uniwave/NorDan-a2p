// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using Serilog;
using Serilog.Exceptions;

namespace a2p.Shared.Infrastructure.Services.Logger
{
    public static class LoggerSetup
    {
        public static void ConfigureLogger(IConfiguration configuration)
        {
            string folder = configuration["AppSettings:Folders:RootFolder"] ?? string.Empty;
            string log = configuration["AppSettings:Folders:Log"] ?? string.Empty;
            string file = Path.Combine(folder, log, "a2pLog.json");

            string source = configuration["Serilog:WriteTo:1:Args:source"] ?? "Uniwave";
            string logName = configuration["Serilog:WriteTo:1:Args:logName"] ?? "Any2PrefSuite";
            _ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, logName);
                Console.WriteLine($"Event source '{source}' created in log '{logName}'.");
            }
            Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(configuration)
             .Enrich.With(new RenderedMessageEnricher()) // Add the custom enricher
             .Enrich.WithExceptionDetails() // Add the custom enricher
             .WriteTo.File(
             formatter: new Serilog.Formatting.Json.JsonFormatter(), // Use the JsonFormatter directly
              path: file, // Specify the log file path
              rollingInterval: RollingInterval.Infinite, // Specify the rolling interval
              shared: true, // Allow multiple processes to write to the same file
              rollOnFileSizeLimit: false // Don't roll on file size limit
          )
             .CreateLogger();
        }
    }
}