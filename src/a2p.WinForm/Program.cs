using a2p.Shared;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Diagnostics;
using System.Text;

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
            _ = _services.GetRequiredService<IWriteMaterialService>();
            _ = _services.GetRequiredService<IWriteItemService>();
            IOrderReadProcessor readService = _services.GetRequiredService<IOrderReadProcessor>();
            IExcelReadService excelReadService = _services.GetRequiredService<IExcelReadService>();
            IFileService fileService = _services.GetRequiredService<IFileService>();



            IOrderWriteProcessor orderProcessingService = _services.GetRequiredService<IOrderWriteProcessor>();



            logService.Information("Application started.");

            using SplashScreenForm splashScreen = new();
            splashScreen.Show();
            splashScreen.FadeIn();
            Task.Delay(2000).Wait();

            MainForm mainWindow = new(readService, excelReadService, orderProcessingService, configuration, logService, fileService);

            splashScreen.FadeOut();
            splashScreen.Close();

            Application.Run(mainWindow);
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