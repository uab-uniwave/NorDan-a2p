using System.Data;

using a2p.Application.Interfaces.Excel;
using a2p.Application.Interfaces.Files;
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
using Microsoft.Extensions.Logging;

using Serilog;

namespace a2p.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        // Get the current culture of the PC
        // Expose a method that returns IServiceCollection so callers (UI project) can register additional types before building
        public static IServiceCollection ConfigureServicesCollection()
        {
            // Load configuration
            IConfiguration configuration = BuildConfiguration();

            // Register services
            ServiceCollection services = new();

            LoggerConfiguration loggerConfig = new LoggerConfiguration()
              .ReadFrom.Configuration(configuration)
              .Enrich.FromLogContext();

            // Initialize Serilog logger instance before registering logging providers
            Log.Logger = loggerConfig.CreateLogger();

            // Register logging (uses Serilog instance)
            _ = services.AddLogging(builder => builder.AddSerilog(Log.Logger, dispose: true));

            // Register a non-generic ILogger so existing code that asks for ILogger (non-generic)
            // will resolve to a logger with the "Application" category.
            _ = services.AddSingleton<Microsoft.Extensions.Logging.ILogger>(sp =>
                sp.GetRequiredService<ILoggerFactory>().CreateLogger("Application"));

            // NOTE: Do NOT register the concrete Logger<> type manually.
            // AddLogging already sets up resolution for ILogger<T>.
            // _ = services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));    // removed — caused unresolved service for Logger<T>

            // Register configuration instance
            _ = services.AddSingleton<IConfiguration>(configuration);
            // Repositories - use Scoped lifetime (per-request) instead of transient/singleton
            _ = services.AddScoped<IDbConnection>(sp =>
            {
                IDbConnectionFactory factory = sp.GetRequiredService<IDbConnectionFactory>();
                IDbConnection conn = factory.CreateConnection();
                conn.Open();
                return conn;
            });
            // Register core services
            _ = services.AddSingleton<ISettingsService, SettingsService>();
            _ = services.AddSingleton<ISQLService, SQLService>();
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
            _ = services.AddSingleton<ITaskQueueService, TaskQueueService>();

            // Register DapperService as singleton with connection string from configuration
            _ = services.AddSingleton<DapperService>();

            _ = services.AddScoped<IMaterialRepository, MaterialRepository>();
            _ = services.AddScoped<IItemRepository, ItemRepository>();
            _ = services.AddScoped<IOrderRepository, OrderRepository>();
            _ = services.AddScoped<ITaskQueueRepository, TaskQueueRepository>();

            // Parsers
            _ = services.AddSingleton<IExcelParserTechDesign, ExcelParserTechDesign>();
            _ = services.AddSingleton<IExcelParserSchuco, ExcelParserSchuco>();
            // TODO: If you have MapperSapa, register it here as well

            return services;
        }

        // Backward-compatible method used by callers who expect IServiceProvider
        public static IServiceProvider ConfigureServices()
        {
            return ConfigureServicesCollection().BuildServiceProvider();
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