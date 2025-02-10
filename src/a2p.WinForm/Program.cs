using a2p.Shared;
using a2p.Shared.Core.Interfaces.Services;
using a2p.Shared.Core.Interfaces.Services.Other.Mappers;

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


            IWriteService writeService = _services.GetRequiredService<IWriteService>();
            IPrefService prefService = _services.GetRequiredService<IPrefService>();
            IFileService fileService = _services.GetRequiredService<IFileService>();
            IReadService readService = _services.GetRequiredService<IReadService>();

            IMappingHandlerService mappingHandlerService = _services.GetRequiredService<IMappingHandlerService>();
            IMapperServiceSapa_V2 materialMapper = _services.GetRequiredService<IMapperServiceSapa_V2>();

            IA2POrderMapper orderMapper = _services.GetRequiredService<IA2POrderMapper>();

            logService.Information("Application started.");

            using SplashScreenForm splashScreen = new();
            splashScreen.Show();
            splashScreen.FadeIn();

            Task.Delay(2000).Wait(); // Ensure splash screen is shown for at least 4 seconds

            MainForm mainWindow = new(fileService, readService, mappingHandlerService, configuration, logService, orderMapper);

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