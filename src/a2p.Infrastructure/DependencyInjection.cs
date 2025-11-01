using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Excel;
using Application.Interfaces.Files;
using Application.Interfaces.Orchestrators;
using Application.Interfaces.PrefSuite;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
// Added for AutoMapper
using Application.Mapping;
using Application.Validations;

using FluentValidation;

using Infrastructure.Data;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Services;
using Infrastructure.Repositories;
using Infrastructure.Services.ExcelServices;
using Infrastructure.Services.FileServices;
using Infrastructure.Services.Orchestartors;
using Infrastructure.Services.PrefSuiteServices;
using Infrastructure.Services.SettingsService;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers all Infrastructure layer dependencies
        /// This method is called by BOTH WinForms and API projects
        /// </summary>
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            // ============================================================
            // 1. Register IDbConnectionFactory (Singleton)
            // ============================================================
            services.AddSingleton<IDbConnectionFactory>(serviceProvider =>
            {
                string? connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    ILogger<SqlConnectionFactory> logger = serviceProvider.GetRequiredService<ILogger<SqlConnectionFactory>>();
                    logger.LogCritical("Connection string 'DefaultConnection' is missing");
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                }

                ILogger<SqlConnectionFactory> logger2 = serviceProvider.GetRequiredService<ILogger<SqlConnectionFactory>>();
                logger2.LogInformation("SqlConnectionFactory registered successfully");

                return new SqlConnectionFactory(connectionString);
            });

            // ============================================================
            // 2. Register DapperService (Scoped)
            // ============================================================
            services.AddScoped<DapperService>();
            services.AddScoped<ISQLService, SQLService>(); //TODO: Change To Dapper 

            // ============================================================
            // 3. Register Repositories (Scoped)
            // ============================================================
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<ITaskQueueRepository, TaskQueueRepository>();
            services.AddScoped<IPrefSuiteRepository, PrefSuiteRepository>();

            // ============================================================
            // 4. Register Application Services (Scoped) DATA SERVICES
            // ============================================================
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IPrefSuiteDataService, PrefSuiteDataService>();
            services.AddScoped<IPrefSuiteAppService, PrefSuiteAppService>();

            // ============================================================
            // 4. Register Application Services (Scoped) File SERVICES
            // ============================================================
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IExcelParserSchuco, ExcelParserSchuco>();
            services.AddScoped<IExcelParserTechDesign, ExcelParserTechDesign>();

            // ============================================================
            // 5. Register Validator Services (Transient) 
            // ============================================================
            services.AddTransient<IValidator<OrderDto>, OrderDtoValidator>();
            services.AddTransient<IValidator<ItemDto>, ItemDtoValidator>();
            services.AddTransient<IValidator<MaterialDto>, MaterialDtoValidator>();
            services.AddTransient<IValidator<TaskDto>, TaskDtoValidator>();

            // ============================================================
            // 6. Register AutoMapper profiles
            // Ensure the AutoMapper.Extensions.Microsoft.DependencyInjection package is referenced.
            // Register profiles explicitly via configuration delegate to avoid overload ambiguity.
            // ============================================================
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MaterialMappingProfile>();
                cfg.AddProfile<ItemMappingProfile>();
                cfg.AddProfile<OrderMappingProfile>();
                cfg.AddProfile<TaskMappingProfile>();
            });

            services.AddScoped<ISettingsService, SettingsService>();

            services.AddScoped<IReadService, ReadService>();
            services.AddScoped<IWriteService, WriteService>();

            // ============================================================

            return services;
        }
    }
}