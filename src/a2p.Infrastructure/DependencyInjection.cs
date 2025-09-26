using Microsoft.Extensions.DependencyInjection;
using System.Configuration.Abstractions;  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a2p.Infrastructure
{
}   
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection s, IConfiguration cfg) //Todo: IServiceCollection
    {
        s.AddDbContext<AppDbContext>(o => o.UseSqlServer(cfg.GetConnectionString("DefaultConnecion")));
        s.AddScoped<IUnitOfWork, EfUnitOfWork>();
        s.AddScoped<IOrderRepository, EfOrderRepository>();
        s.AddScoped<IExcelReader, ClosedXmlExcelReader>();
        s.AddScoped<IThirdPartyRunner, ExternalProcessRunner>();
        return s;
    }
}