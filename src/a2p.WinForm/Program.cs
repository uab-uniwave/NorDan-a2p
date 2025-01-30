using System.Diagnostics;
using System.Text;

using a2p.Shared;
using a2p.Shared.Core.Interfaces.Mappers;
using a2p.Shared.Core.Interfaces.Services.Import;
using a2p.Shared.Core.Interfaces.Services.Other;
using a2p.Shared.Core.Utils;

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
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2); // ✅ Critical for DPI scaling
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


        _services=DependencyInjection.ConfigureServices();
            IConfiguration configuration = _services.GetRequiredService<IConfiguration>();


            ILogService logger = _services.GetRequiredService<ILogService>();
            Console.SetOut(new DebugTextWriter());

            IFileService fileService = _services.GetRequiredService<IFileService>();
            IExcelService excelService = _services.GetRequiredService<IExcelService>();
            IImportService importService = _services.GetRequiredService<IImportService>();
            IOrderMapper orderMapper = _services.GetRequiredService<IOrderMapper>();

            logger.Information("Application started.");

            using SplashScreenForm splashScreen = new();
            splashScreen.Show();
            splashScreen.FadeIn();

            Task.Delay(4000).Wait(); // Ensure splash screen is shown for at least 4 seconds

            MainForm mainWindow = new(fileService, excelService, importService, configuration, logger, orderMapper);

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