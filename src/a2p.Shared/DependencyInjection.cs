// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services;
using a2p.Shared.Infrastructure.Interfaces;
using a2p.Shared.Infrastructure.Services;
using a2p.Shared.Infrastructure.Services.Logger;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace a2p.Shared
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
            _ = services.AddSingleton<DataCache>();
            _ = services.AddSingleton<IUserSettingsService, UserSettingsService>();
            _ = services.AddSingleton<SettingsManager>();
            _ = services.AddSingleton<IWriteService, WriteService>();
            _ = services.AddSingleton<IReadService, ReadService>();
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
