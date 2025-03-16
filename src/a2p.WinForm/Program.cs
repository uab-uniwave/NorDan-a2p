// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Text;

using a2p.Shared;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Infrastructure.Interfaces;
using a2p.Shared.Infrastructure.Services;

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
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2); // ✅ Critical for DPI scaling
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _services = DependencyInjection.ConfigureServices();
            IConfiguration configuration = _services.GetRequiredService<IConfiguration>();

            ILogService logService = _services.GetRequiredService<ILogService>();
            Console.SetOut(new DebugTextWriter());
            _ = _services.GetRequiredService<IPrefSuiteService>();

            _ = _services.GetRequiredService<IMapperSapaV1>();
            _ = _services.GetRequiredService<IMapperSapaV2>();
            _ = _services.GetRequiredService<IMapperSchuco>();

            _ = _services.GetRequiredService<IWriteService>();
            _ = _services.GetRequiredService<SettingsManager>();
            _ = _services.GetRequiredService<IUserSettingsService>();

            DataCache dataCache = _services.GetRequiredService<DataCache>();
            IOrderReadProcessor readService = _services.GetRequiredService<IOrderReadProcessor>();
            IExcelReadService excelReadService = _services.GetRequiredService<IExcelReadService>();
            IFileService fileService = _services.GetRequiredService<IFileService>();
            IWriteService writeService = _services.GetRequiredService<IWriteService>();
            IUserSettingsService userSettingsService = _services.GetRequiredService<IUserSettingsService>();
            logService.Information("Application started.");

            using SplashScreenForm splashScreen = new();
            splashScreen.Show();
            splashScreen.FadeIn();
            Task.Delay(2000).Wait();

            MainForm mainWindow = new(readService, excelReadService, writeService, configuration, logService, fileService, dataCache, userSettingsService);

            splashScreen.FadeOut();
            splashScreen.Close();

            Application.Run(mainWindow);
        }
    }

    public class DebugTextWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string? message) => Debug.WriteLine(message);
    }
}
