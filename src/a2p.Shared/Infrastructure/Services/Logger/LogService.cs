// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Nodes;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Extensions.Configuration;

using Serilog;

namespace a2p.Shared.Infrastructure.Services.Logger
{
    public class LogService : ILogService
    {
        private readonly ILogger _logService = Log.Logger;
        private readonly IConfiguration _configuration;
        private string _file;
        private readonly ConcurrentQueue<A2PLogRecord> Records = new();
        private readonly object FileLock = new(); // Lock for thread-safe writes

        public LogService(IConfiguration configuration)
        {

            _configuration = configuration;
            _file = Path.Combine(_configuration["AppSettings:Folders:RootFolder"] ?? "C://Temp//Import", _configuration["AppSettings:Folders:Log"] ?? "Log", "a2pLog.json");
        }

        public void Verbose(string message, params object[]? args) => _logService.Verbose(message ?? "Message missing", args);

        public void Verbose(Exception ex, string? message, params object[]? args) => _logService.Verbose(ex, message ?? "Message missing", args);

        public void Verbose(Exception ex) => _logService.Verbose("{$Exception}", ex);
        public void Debug(string message, params object[]? args) => _logService.Debug(message ?? "Message missing", args);

        public void Debug(Exception ex, string message, params object[]? args) => _logService.Debug(ex, message ?? "Message missing", args);

        public void Debug(Exception ex) => _logService.Debug("{$Exception}", ex);

        public void Information(string message, params object[]? args) => _logService.Information(message ?? "Message missing", args);

        public void Information(Exception ex, string message, params object[]? args) => _logService.Information(ex, message, args);

        public void Information(Exception ex) => _logService.Information("{$Exception}", ex);

        public void Warning(string message, params object[]? args) => _logService.Warning(message, args);

        public void Warning(Exception ex, string message, params object[]? args) => _logService.Warning(ex, message, args);

        public void Warning(Exception ex) => _logService.Warning("{$Exception}", ex);

        public void Error(string message, params object[]? args) => _logService.Error(message, args);

        public void Error(Exception ex, string message, params object[]? args) => _logService.Error(ex, message, args);

        public void Error(Exception ex) => _logService.Error("{$Exception}", ex);

        public void Fatal(string message, params object[]? args) => _logService.Fatal(message, args);

        public void Fatal(Exception ex, string message, params object[]? args) => _logService.Fatal(ex, message, args);

        public void Fatal(Exception ex) => _logService.Verbose("{$Exception}", ex);
        public async Task<List<A2PLogRecord>> GetRepository(string fileName)
        {

            List<A2PLogRecord> logEntries = [];
            if (!string.IsNullOrEmpty(fileName))
            {
                _file = fileName;
            }

            if (_file == null)
            {
                return logEntries;
            }

            // JsonSerializer options for flexibility
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true, // Case-insensitive matching
                AllowTrailingCommas = true,   // Allow extra commas in JSON
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            // Read and process each line of the JSON file
            int lineNumber = 0; // Line counter for better error tracing
            using (FileStream fileStream = new(_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader streamReader = new(fileStream))
            {

                try
                {
                    string? line;
                    while ((line = await streamReader.ReadLineAsync()) != null)
                    {
                        lineNumber++;
                        try
                        {
                            JsonNode? jsonNode = JsonNode.Parse(line);
                            if (jsonNode == null)
                            {
                                _logService.Warning("FS: Error getting repository,cant pars to JSON log file line {0}: {1}", lineNumber, line);
                                continue;
                            }

                            if (jsonNode["Properties"] is not JsonObject propertiesNode)
                            {
                                _logService.Warning("FS: Error getting repository, Properties node is null in log file line {0}: {1}", lineNumber, line);
                                continue;
                            }

                            A2PLogRecord logEntry = new()
                            {
                                Timestamp = jsonNode["Timestamp"]?.ToString() ?? string.Empty,
                                Level = jsonNode["Level"]?.ToString() ?? string.Empty,
                                Message = propertiesNode["RenderedMessage"]?.ToString() ?? string.Empty,
                                Exception = propertiesNode["Exception"]?.ToString() ?? string.Empty,
                                Order = propertiesNode["Order"]?.ToString() ?? string.Empty,
                                Worksheet = propertiesNode["Worksheet"]?.ToString() ?? string.Empty,
                                Reference = propertiesNode["Reference"]?.ToString() ?? string.Empty,
                                Color = propertiesNode["Color"]?.ToString() ?? string.Empty,
                                // Convert Properties node to a Dictionary
                                Properties = propertiesNode.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() as object) // Convert JsonNode to string
                            };

                            logEntries.Add(logEntry);
                        }

                        catch (Exception ex)
                        {
                            // Log detailed errors with line numbers
                            _logService.Warning("FS: Unhandled Error getting repository, error parsing log file line {0}: {1}", lineNumber, ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logService.Warning("FS: Error getting log Stream Lin : {0}", ex.Message); return [];
                }
            }

            return logEntries;
        }

        #region -== Additional Methods ==-
        public void DeleteLogFiles()
        {
            try
            {
                string folder = _configuration["AppSettings:Folders:RootFolder"] ?? string.Empty;
                string log = _configuration["AppSettings:Folders:Log"] ?? string.Empty;
                string file = Path.Combine(folder, log, "a2pLog.json");
                string fileCopy = Path.Combine(folder, log, $"a2p-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json");
                _ = CloseAndFlush();

                //Make Copy and Delete current a2p.json log file.
                //==============================================================
                if (!string.IsNullOrEmpty(folder) && !string.IsNullOrEmpty(log))
                {
                    if (File.Exists(file))
                    {
                        File.Copy(file, fileCopy, true);
                        File.Delete(file);
                    }
                }

                //Delete all log files older then 30 days
                //==============================================================
                string[] logFiles = Directory.GetFiles(Path.Combine(folder, log));
                foreach (string oldFile in logFiles)
                {
                    DateTime creationTime = File.GetCreationTime(oldFile);
                    if (creationTime < DateTime.Now.AddDays(-30)) // Delete logs older than 30 days
                    {
                        File.Delete(oldFile);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString(), "Error while deleting log files");
            }
        }
        public async Task<bool> CloseAndFlush()
        {
            try
            {
                ValueTask result = await Task.Run(() => Log.CloseAndFlushAsync());
                return true;
            }
            catch (Exception ex)
            {
                _logService.Warning("FS: Error closing and flushing log file: {0}", ex.Message);
                return false;

            }

            #endregion -== Additional Methods ==-

        }

        void ILogService.CloseAndFlush() => throw new NotImplementedException();
    }
}
