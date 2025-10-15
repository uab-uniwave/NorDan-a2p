using a2p.Application.Interfaces;
using a2p.WinForm.Forms;

using Microsoft.Extensions.DependencyInjection;

using System.Diagnostics;
using System.Globalization;
using System.Text;

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

            _ = System.Windows.Forms.Application.SetHighDpiMode(System.Windows.Forms.HighDpiMode.PerMonitorV2);
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            _services = ConfigureServices();

            var logService = _services.GetRequiredService<ILogService>();
            Console.SetOut(new DebugTextWriter());

            var settingsService = _services.GetRequiredService<ISettingsService>();
            var excelService = _services.GetRequiredService<IExcelService>();
            var readService = _services.GetRequiredService<IReadService>();
            var fileService = _services.GetRequiredService<IExcelService>();
            var writeService = _services.GetRequiredService<IWriteService>();

            logService.Information("Application started.");

            using var splashScreen = new FormSplashScreen();
            splashScreen.Show();
            splashScreen.FadeIn();
            Task.Delay(2000).Wait();

            var mainForm = new FormMain(readService, excelService, logService, fileService, settingsService, writeService);

            splashScreen.FadeOut();
            splashScreen.Close();

            System.Windows.Forms.Application.Run(mainForm);
        }

        private static IServiceProvider ConfigureServices()
        {
            // Use the centralized DI from Infrastructure
            return a2p.Infrastructure.DependencyInjection.ConfigureServices();
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
