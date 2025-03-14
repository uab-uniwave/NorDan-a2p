using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services;
using a2p.Shared.Infrastructure.Interfaces;
using a2p.Shared.Infrastructure.Services;
using a2p.Shared.Infrastructure.Services.Logger;

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

            // Initialize Serilog
            LoggerSetup.ConfigureLogger(configuration);

            // Register logging
            _ = services.AddLogging(builder => builder.AddSerilog());

            // Register configuration instance
            _ = services.AddSingleton<IConfiguration>(configuration);

            // Register core services
            _ = services.AddSingleton<ILogService, LogService>();
            _ = services.AddSingleton<DataCache>();
            _ = services.AddSingleton<IUserSettingsService, UserSettingsService>();
            _ = services.AddSingleton<SettingsManager>();
            _ = services.AddSingleton<IOrderReadProcessor, OrderReadProcessor>();
            _ = services.AddSingleton<IExcelReadService, ExcelReadService>();
            _ = services.AddSingleton<IPrefSuiteService, PrefSuiteService>();
            _ = services.AddSingleton<IWriteItemService, WriteItemService>();
            _ = services.AddSingleton<IWriteMaterialService, WriteMaterialService>();
            _ = services.AddSingleton<IFileService, FileService>();
            _ = services.AddSingleton<IMapperSapaV1, MapperSapaV1>();
            _ = services.AddSingleton<IMapperSapaV2, MapperSapaV2>();
            _ = services.AddSingleton<IMapperSchuco, MapperSchuco>();
            _ = services.AddSingleton<IOrderWriteProcessor, OrderWritingProcessor>();
            _ = services.AddSingleton<ISqlRepository, SqlRepository>();

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
    }
}
