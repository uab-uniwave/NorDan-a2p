using a2p.Application.Interfaces;
using a2p.Application.Services;
using a2p.Domain.Interfaces;
using a2p.Infrastructure.Data;
using a2p.Infrastructure.Services;
using a2p.Infrastructure.Services.Logger;
using a2p.Infrastructure.Services.MappingService;
using a2p.Infrastructure.Services.PrefSuiteService;
using a2p.Infrastructure.Services.SettingsService;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace a2p.Infrastructure
{
    public static class DependencyInjection
    {
        // Get the current culture of the PC
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
            _ = services.AddSingleton<ISettingsService, SettingsService>();
            _ = services.AddSingleton<SettingsManager>();
            // _ = services.AddSingleton<IExcelService, Services.ExcelService>();
            _ = services.AddSingleton<ISQLService, SQLService>();
            _ = services.AddSingleton<IPrefSuiteService, PrefSuiteService>();
            _ = services.AddSingleton<IPrefSuiteDataService, PrefSuiteDataService>();
            _ = services.AddSingleton<IFileService, FileService>();

            // Application services
            _ = services.AddSingleton<IReadService, ReadService>();
            _ = services.AddSingleton<IWriteService, WriteService>();
            _ = services.AddSingleton<IOrderService, OrderService>();
            _ = services.AddSingleton<IMaterialService, MaterialService>();
            _ = services.AddSingleton<IItemService, ItemService>();

            // Get connection string from configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration");
            }

            // Register DapperService as singleton with connection string from configuration
            _ = services.AddSingleton<DapperService>(provider => new DapperService(connectionString));

            // Repositories
            _ = services.AddSingleton<IMaterialRepository, MaterialRepository>();
            _ = services.AddSingleton<IItemRepository, ItemRepository>();
            
            // Register OrderQueueRepository with its dependencies
            _ = services.AddSingleton<IOrderQueueRepository>(provider =>
            {
                var logService = provider.GetRequiredService<ILogService>();
                return new OrderQueueRepository(connectionString, logService);
            });

            // Mappers
            _ = services.AddSingleton<IExcelParserTechDesign, ExcelParserTechDesign>();
            _ = services.AddSingleton<IExcelParserSchuco, ExcelParserSchuco>();
            // TODO: If you have MapperSapa, register it here as well

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
