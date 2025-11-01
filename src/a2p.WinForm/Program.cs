using Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WinFormApp.Forms;

namespace WinFormApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // ============================================================
            // 1. Build Configuration from appsettings.json
            // ============================================================
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .Build();

            // ============================================================
            // 2. Create Host with Dependency Injection
            // ============================================================
            IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Register configuration as singleton
                services.AddSingleton<IConfiguration>(configuration);

                // Configure logging from appsettings.json
                services.AddLogging(builder =>
     {
         builder.ClearProviders();
         builder.AddConfiguration(configuration.GetSection("Logging"));
         builder.AddConsole();
         builder.AddDebug();
         builder.AddEventLog();
     });

                // ⭐ Register Infrastructure services (SHARED WITH API!)
                services.AddInfrastructure(configuration);

                // Register Forms as transient (new instance each time)
                services.AddTransient<MainForm>();
                services.AddTransient<LogsForm>();
                services.AddTransient<ProgressBarForm>();
                services.AddTransient<SettingsForm>();
                services.AddTransient<OrdersForm>();
                services.AddTransient<SplashScreenForm>();

            })
            .Build();

            // ============================================================
            // 3. Get MainForm from DI container and run application
            // ============================================================
            IServiceScope scope = host.Services.CreateScope();
            try
            {
                MainForm mainForm = scope.ServiceProvider.GetRequiredService<MainForm>();
                System.Windows.Forms.Application.Run(mainForm);
            }
            finally
            {
                // Dispose scope after Application.Run returns (when app exits)
                scope.Dispose();
                host.Dispose();
            }
        }
    }
}