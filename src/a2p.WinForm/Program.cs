using a2p.Application.Interfaces;
using a2p.Application.Interfaces.MappingService;
using a2p.Domain.Interfaces;
using a2p.Infrastructure.Data;
using a2p.Infrastructure.Services;
using a2p.Infrastructure.Services.Logger;
using a2p.Infrastructure.Services.MappingService;
using a2p.Infrastructure.Services.PrefSuiteService;
using a2p.Infrastructure.Services.SettingsService;
using a2p.WinForm.Forms;

using Microsoft.Extensions.Configuration;
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
            var fileService = _services.GetRequiredService<IFileService>();
            var writeService = _services.GetRequiredService<IWriteService>();
            var orderRepository = _services.GetRequiredService<IMyRepository>();

            logService.Information("Application started.");

            using var splashScreen = new SplashScreenForm();
            splashScreen.Show();
            splashScreen.FadeIn();
            Task.Delay(2000).Wait();

            var mainForm = new FormMain(readService, excelService, orderRepository, logService, fileService, settingsService, writeService);

            splashScreen.FadeOut();
            splashScreen.Close();

            System.Windows.Forms.Application.Run(mainForm);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            // Register services
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IExcelService, ExcelReaderService>();
            services.AddSingleton<IReadService, ReadService>();
            services.AddSingleton<IWriteService, WriteService>();
            services.AddSingleton<IPrefSuiteService, PrefSuiteService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IMyRepository, MyRepository>();
            services.AddSingleton<ISQLService, SQLService>();
            services.AddSingleton<SettingsManager>();

            // Register mappers
            services.AddSingleton<IMapperTechDesign, MapperTechDesign>();
            services.AddSingleton<IMapperSchuco, MapperSchuco>();

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
