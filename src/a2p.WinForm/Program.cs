using a2p.Shared;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services;
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
            _ = _services.GetRequiredService<IWriteMaterials>();
            _ = _services.GetRequiredService<IPrefSuiteIntegrationService>();
            IFileService fileService = _services.GetRequiredService<IFileService>();
            IExcelReadService readService = _services.GetRequiredService<IExcelReadService>();

            IOrderProcessingService mappingHandlerService = _services.GetRequiredService<IOrderProcessingService>();
            _ = _services.GetRequiredService<IMapperSapaV2>();

            IA2POrderMapper orderMapper = _services.GetRequiredService<IA2POrderMapper>();

            logService.Information("Application started.");

            using SplashScreenForm splashScreen = new();
            splashScreen.Show();
            splashScreen.FadeIn();
            Task.Delay(2000).Wait();

            MainForm mainWindow = new(fileService, readService, mappingHandlerService, configuration, logService, orderMapper);

            //splashScreen.FadeOut();
            //splashScreen.Close();




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