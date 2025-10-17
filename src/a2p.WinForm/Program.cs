using System.Diagnostics;
using System.Globalization;
using System.Text;

using a2p.Infrastructure.DependencyInjection;
using a2p.WinForm.Forms;

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

            _ = System.Windows.Forms.Application.SetHighDpiMode(System.Windows.Forms.HighDpiMode.PerMonitorV2);
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            IServiceCollection services = DependencyInjection.ConfigureServicesCollection();

            // Register WinForm-specific types explicitly so ActivatorUtilities isn't needed
            _ = services.AddSingleton<FormSplashScreen>();
            _ = services.AddSingleton<FormMain>();
            _ = services.AddTransient<ChildFormOrders>();
            _ = services.AddTransient<ChildFormLog>();
            _ = services.AddTransient<ChildFormSetting>();
            _ = services.AddTransient<FormProgressBar>();
            services.AddLogging(builder => builder.AddConsole());

            _services = services.BuildServiceProvider();

            Microsoft.Extensions.Logging.ILogger logger = _services.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();
            Console.SetOut(new DebugTextWriter());


            logger.LogInformation("Application started.");

            using FormSplashScreen splashScreen = _services.GetRequiredService<FormSplashScreen>();
            splashScreen.Show();
            splashScreen.FadeIn();
            FormMain mainForm = _services.GetRequiredService<FormMain>();

            splashScreen.FadeOut();
            splashScreen.Close();

            System.Windows.Forms.Application.Run(mainForm);
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
