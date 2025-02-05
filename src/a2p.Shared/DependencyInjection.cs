using a2p.Shared.Core.Interfaces.Mappers;
using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Repository.SubSql;
using a2p.Shared.Core.Interfaces.Services.Other;
using a2p.Shared.Core.Interfaces.Services.Read;
using a2p.Shared.Core.Utils;
using a2p.Shared.Infrastructure.Mappers;
using a2p.Shared.Infrastructure.Repositories;
using a2p.Shared.Infrastructure.Repositories.SubSql;
using a2p.Shared.Infrastructure.Services.Other;
using a2p.Shared.Infrastructure.Services.Read;
using a2p.Shared.Infrastructure.Utils.Logger;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using System.Diagnostics;

namespace a2p.Shared
{
    public static class DependencyInjection
    {
        public static IServiceProvider ConfigureServices()
        {
            // Load configuration
            IConfiguration configuration = BuildConfiguration();

            // Register services
            ServiceCollection services = new();


            string rootFolder = configuration["AppSettings:Folders:Root"] ?? @"C:\\Temp\\Import";
            EnsureDirectoryExist(rootFolder, "Root", configuration);
            string importSuccessFolder = Path.Combine(rootFolder, configuration["AppSettings:Folders:ImportSuccess"] ?? "Import_Success");
            EnsureDirectoryExist(importSuccessFolder, "ImportSuccess", configuration);
            string importFailedFolder = Path.Combine(rootFolder, configuration["AppSettings:Folders:ImportFailed"] ?? "Import_Failed");
            EnsureDirectoryExist(importFailedFolder, "ImportFailed", configuration);
            string logFolder = Path.Combine(rootFolder, configuration["AppSettings:Folders:Log"] ?? "Log");
            EnsureDirectoryExist(logFolder, "Log", configuration);
            string logFile = Path.Combine(logFolder, "a2pLog.json");

            try
            {

                bool fileExists = false;

                if (File.Exists(logFile))
                {
                    fileExists = true;
                }


                if (fileExists)
                {
                    File.Delete(logFile);
                }
            }

            catch (IOException ex)
            {
                Debug.WriteLine($"PR. Deleting file: {logFile} failed. Exception: {ex.Message}");
                throw;
            }

            // Initialize Serilog
            LoggerSetup.ConfigureLogger(configuration);



            // Register logging
            _ = services.AddLogging(builder => builder.AddSerilog());

            // Register configuration instance
            _ = services.AddSingleton<IConfiguration>(configuration);

            // Register core services
            _ = services.AddSingleton<ILogService, LogService>();
            _ = services.AddSingleton<IExcelService, ExcelService>();
            _ = services.AddSingleton<IFileService, FileService>();
            _ = services.AddSingleton<IItemMapper, ItemMapper>();
            _ = services.AddSingleton<IMaterialMapper, MaterialMapper>();
            _ = services.AddSingleton<IOrderMapper, OrderMapper>();
            _ = services.AddSingleton<IGlassMapper, GlassMapper>();
            _ = services.AddSingleton<IPanelMapper, PanelMapper>();
            _ = services.AddSingleton<IReadSapa_v1, ReadSapa_v1>();
            _ = services.AddSingleton<IReadSapa_v2, ReadSapa_v2>();
            _ = services.AddSingleton<IReadSchuco, ReadSchuco>();
            _ = services.AddSingleton<IReadService, ReadService>();
            _ = services.AddSingleton<ISqlService, SqlService>();
            _ = services.AddSingleton<ISqlSapa_v1, SqlSapa_v1>();
            _ = services.AddSingleton<ISqlSapa_v2, SqlSapa_v2>();
            _ = services.AddSingleton<ISqlSchuco, SqlSchuco>();


            return services.BuildServiceProvider();
        }

        private static IConfiguration BuildConfiguration()
        {
            string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

            return new ConfigurationBuilder()
             .SetBasePath(AppContext.BaseDirectory)
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile($"appsettings.{environment}.json", optional: true)
             .Build();
        }
        private static void EnsureDirectoryExist(string directoryPath, string folderKey, IConfiguration configuration)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Debug.WriteLine($"FS. Directory does not exist. Creating directory: {directoryPath}");
                    _ = Directory.CreateDirectory(directoryPath);
                    configuration[$"AppSettings:Folders:{folderKey}"] = directoryPath;
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine($"FS. Creating directory: {directoryPath} failed. Exception: {ex.Message}");
            }
        }
    }
}