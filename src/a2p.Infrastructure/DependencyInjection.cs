// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using a2p.Application.Services;
using a2p.Application.Services.MappingService;
using a2p.Domain.Respoitories;
using a2p.Infrastructure.Services;
using a2p.Infrastructure.Services.Logger;
using a2p.Infrastructure.Services.MappingService;
using a2p.Infrastructure.Services.SettingsService;
using a2p.Infrastructure.Services.SQLService;

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
            _ = services.AddSingleton<IExcelService, ExcelService>();
            _ = services.AddSingleton<IPrefSuiteService, PrefSuiteService>();
            _ = services.AddSingleton<ISQLRepository, SQLRepository>();
            _ = services.AddSingleton<IFileService, FileService>();
            _ = services.AddSingleton<IMapperSapa, MapperSapa>();
            _ = services.AddSingleton<IMapperTechDesign, MapperTechDesign>();
            _ = services.AddSingleton<IMapperSchuco, MapperSchuco>();
            _ = services.AddSingleton<ISQLService, SQLService>();

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
