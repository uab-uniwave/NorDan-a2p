using a2p.Application.Interfaces.Excel;
using a2p.Application.Interfaces.Excel.Files;
using a2p.Application.Interfaces.Orchestrators;
using a2p.Application.Interfaces.PrefSuite;
using a2p.Application.Interfaces.Repositories;
using a2p.Application.Interfaces.Services;
using a2p.Application.Services;
using a2p.Infrastructure.Persistence.Repositories;
using a2p.Infrastructure.Services.DataServices;
using a2p.Infrastructure.Services.ExcelServices;
using a2p.Infrastructure.Services.FileServices;
using a2p.Infrastructure.Services.Orchestartors;
using a2p.Infrastructure.Services.PrefSuiteServices;
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

            LoggerConfiguration loggerConfig = new LoggerConfiguration()
              .ReadFrom.Configuration(configuration)
              .Enrich.FromLogContext();

            // Register logging
            _ = services.AddLogging(builder => builder.AddSerilog());

            // Register configuration instance
            _ = services.AddSingleton<IConfiguration>(configuration);

            // Register core services
            _ = services.AddSingleton<ISettingsService, SettingsService>();
            _ = services.AddSingleton<SettingsManager>();
            _ = services.AddSingleton<IExcelService, ExcelService>();

            _ = services.AddSingleton<IPrefSuiteService, PrefSuiteService>();
            _ = services.AddSingleton<IPrefSuiteDataService, PrefSuiteDataService>();
            _ = services.AddSingleton<IFileService, FileService>();

            // Application services
            _ = services.AddSingleton<IReadService, ReadService>();
            _ = services.AddSingleton<IWriteService, WriteService>();
            _ = services.AddSingleton<IOrderService, OrderService>();
            _ = services.AddSingleton<IMaterialService, MaterialService>();
            _ = services.AddSingleton<IItemService, ItemService>();
            _ = services.AddSingleton<ITaskQueueService, ITaskQueueService>();

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
            _ = services.AddSingleton<IOrderRepository, OrderRepository>();
            _ = services.AddSingleton<ITaskQueueRepository, TaskQueueRepository>();



            // Parsers
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
