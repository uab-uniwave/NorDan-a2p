using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Nodes;

using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Utils;

using Microsoft.Extensions.Configuration;

using Serilog;

namespace a2p.Shared.Infrastructure.Utils.Logger
{
    public class LogService : ILogService
    {
        private readonly ILogger _logger = Log.Logger;
        private readonly IConfiguration _configuration;
        private string _file;
        private readonly ConcurrentQueue<A2PLogGridRecord> Records = new();
        private readonly object FileLock = new(); // Lock for thread-safe writes

        public LogService(IConfiguration configuration)
        {

            _configuration=configuration;
            _file=Path.Combine(_configuration["AppSettings:Folders:RootFolder"]??"C://Temp//Import", _configuration["AppSettings:Folders:Log"]??"Log", "a2pLog.json");
        }

        public void Verbose(string message, params object[]? args)
        {
            _logger.Verbose(message??"Message missing", args);

        }

        public void Verbose(Exception ex, string? message, params object[]? args)
        {
            _logger.Verbose(ex, message??"Message missing", args);

        }

        public void Verbose(Exception ex)
        {
            _logger.Verbose("{@Exception}", ex);
        }
        public void Debug(string message, params object[]? args)
        {
            _logger.Debug(message??"Message missing", args);

        }

        public void Debug(Exception ex, string message, params object[]? args)
        {
            _logger.Debug(ex, message??"Message missing", args);
        }

        public void Debug(Exception ex)
        {
            _logger.Debug("{@Exception}", ex);
        }


        public void Information(string message, params object[]? args)
        {
            _logger.Information(message??"Message missing", args);
        }

        public void Information(Exception ex, string message, params object[]? args)
        {
            _logger.Information(ex, message, args);
        }

        public void Information(Exception ex)
        {
            _logger.Information("{@Exception}", ex);
        }


        public void Warning(string message, params object[]? args)
        {
            _logger.Warning(message, args);
        }

        public void Warning(Exception ex, string message, params object[]? args)
        {
            _logger.Warning(ex, message, args);
        }

        public void Warning(Exception ex)
        {
            _logger.Warning("{@Exception}", ex);
        }

        public void Error(string message, params object[]? args)
        {
            _logger.Error(message, args);
        }

        public void Error(Exception ex, string message, params object[]? args)
        {
            _logger.Error(ex, message, args);
        }

        public void Error(Exception ex)
        {
            _logger.Error("{@Exception}", ex);
        }

        public void Fatal(string message, params object[]? args)
        {
            _logger.Fatal(message, args);
        }

        public void Fatal(Exception ex, string message, params object[]? args)
        {
            _logger.Fatal(ex, message, args);
        }

        public void Fatal(Exception ex)
        {
            _logger.Verbose("{@Exception}", ex);
        }
        public async Task<List<A2PLogGridRecord>> GetRepository(string fileName)
        {

            List<A2PLogGridRecord> logEntries = [];
            if (!string.IsNullOrEmpty(fileName))
            {
                _file=fileName;
            }

            if (_file==null)
            {
                return logEntries;
            }

            // JsonSerializer options for flexibility
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive=true, // Case-insensitive matching
                AllowTrailingCommas=true,   // Allow extra commas in JSON
                ReadCommentHandling=JsonCommentHandling.Skip
            };

            // Read and process each line of the JSON file
            int lineNumber = 0; // Line counter for better error tracing
            using (FileStream fileStream = new(_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader streamReader = new(fileStream))
            {

                try
                {
                    string? line;
                    while ((line=await streamReader.ReadLineAsync())!=null)
                    {
                        lineNumber++;
                        try
                        {
                            JsonNode? jsonNode = JsonNode.Parse(line);
                            if (jsonNode==null)
                            {
                                _logger.Warning("FS: Error getting repository,cant pars to JSON log file line {0}: {1}", lineNumber, line);
                                continue;
                            }


                            if (jsonNode["Properties"] is not JsonObject propertiesNode)
                            {
                                _logger.Warning("FS: Error getting repository, Properties node is null in log file line {0}: {1}", lineNumber, line);
                                continue;
                            }


                            A2PLogGridRecord logEntry = new()
                            {
                                Timestamp=jsonNode["Timestamp"]?.ToString()??string.Empty,
                                Level=jsonNode["Level"]?.ToString()??string.Empty,
                                Message=propertiesNode["RenderedMessage"]?.ToString()??string.Empty,
                                Exception=propertiesNode["Exception"]?.ToString()??string.Empty,
                                Order=propertiesNode["Order"]?.ToString()??string.Empty,
                                Worksheet=propertiesNode["Worksheet"]?.ToString()??string.Empty,
                                Line=propertiesNode["Line"]?.ToString()??string.Empty,
                                // Convert Properties node to a Dictionary
                                Properties=propertiesNode.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() as object) // Convert JsonNode to string
                            };


                            logEntries.Add(logEntry);
                        }


                        catch (Exception ex)
                        {
                            // Log detailed errors with line numbers
                            _logger.Warning("FS: Unhandled Error getting repository, error parsing log file line {0}: {1}", lineNumber, ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Warning("FS: Error getting log Stream Lin : {0}", ex.Message); return [];
                }
            }

            return logEntries;
        }



        #region -== Additional Methods ==-
        public void DeleteLogFiles()
        {
            try
            {
                string folder = _configuration["AppSettings:Folders:RootFolder"]??string.Empty;
                string log = _configuration["AppSettings:Folders:Log"]??string.Empty;
                string file = System.IO.Path.Combine(folder, log, "a2pLog.json");
                string fileCopy = System.IO.Path.Combine(folder, log, $"a2p-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json");
                CloseAndFlush();


                //Make Copy and Delete current a2p.json log file.
                //==============================================================
                if (!string.IsNullOrEmpty(folder)&&!string.IsNullOrEmpty(log))
                {
                    if (File.Exists(file))
                    {
                        File.Copy(file, fileCopy, true);
                        File.Delete(file);
                    }
                }

                //Delete all log files older then 30 days
                //==============================================================
                string[] logFiles = Directory.GetFiles(System.IO.Path.Combine(folder, log));
                foreach (string oldFile in logFiles)
                {
                    DateTime creationTime = File.GetCreationTime(oldFile);
                    if (creationTime<DateTime.Now.AddDays(-30)) // Delete logs older than 30 days
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
        public async void CloseAndFlush()
        {

            await Log.CloseAndFlushAsync();
        }

        #endregion -== Additional Methods ==-


    }
}

