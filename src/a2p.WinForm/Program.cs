// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Globalization;
using System.Text;
using a2p.Application.Interfaces;
using a2p.Shared;
using a2p.Shared.Application.Services;
using a2p.Shared.Infrastructure.Interfaces;
using a2p.Shared.Infrastructure.Services;
using a2p.Shared.Infrastructure.Services.Logger;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace a2p.WinForm
{
    internal static class Program
    {
        private static IServiceProvider _services = null!;

        [STAThread]
        private static void Main()
        {
            // Get the current culture of the PC
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            // Set the culture globally
            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;

            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2); // ✅ Critical for DPI scaling
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _services = DependencyInjection.ConfigureServices();

            _ = _services.GetRequiredService<IConfiguration>();

            ILogService logService = _services.GetRequiredService<ILogService>();
            Console.SetOut(new DebugTextWriter());
            _ = _services.GetRequiredService<IPrefSuiteService>();

            _ = _services.GetRequiredService<IMapperSapa>();
            _ = _services.GetRequiredService<IMapperTechDesign>();
            _ = _services.GetRequiredService<IMapperSchuco>();
            _ = _services.GetRequiredService<SettingsManager>();

            _ = _services.GetRequiredService<DataCache>();

            IUserSettingsService userSettingsService = _services.GetRequiredService<IUserSettingsService>();
            IExcelService excelService = _services.GetRequiredService<IExcelService>();
            IReadService readService = _services.GetRequiredService<IReadService>();
            IFileService fileService = _services.GetRequiredService<IFileService>();
            IWriteService writeService = _services.GetRequiredService<IWriteService>();
            ISQLRepository sqlRepository = _services.GetRequiredService<ISQLRepository>();

            _ = _services.GetRequiredService<ISQLService>();

            _ = _services.GetRequiredService<IFileService>();

            _ = _services.GetRequiredService<ISQLRepository>();

            _ = _services.GetRequiredService<IUserSettingsService>();
            logService.Information("Application started.");

            using SplashScreenForm splashScreen = new();
            splashScreen.Show();
            splashScreen.FadeIn();
            Task.Delay(2000).Wait();
            MainForm mainWindow = new(readService, excelService, sqlRepository, logService, fileService, userSettingsService, writeService);

            splashScreen.FadeOut();
            splashScreen.Close();

            Application.Run(mainWindow);
        }
        public static IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new();

            // Register other services
            _ = services.AddSingleton<ILogService, LogService>();
            _ = services.AddSingleton<IFileService, FileService>();
            _ = services.AddSingleton<IExcelService, ExcelService>();
            _ = services.AddSingleton<IReadService, ReadService>();
            _ = services.AddSingleton<IWriteService, WriteService>(); // Register IWriteService

            // Build the service provider
            return services.BuildServiceProvider();
        }
    }

    public class DebugTextWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string? message)
        {
            Debug.WriteLine(message);
        }
    }
}
