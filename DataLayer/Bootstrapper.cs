using System.Runtime.CompilerServices;
using DataLayer.Storages;
using DataLayer.Storages.DbContexts;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("WebApi.IntegrationTests")]

namespace DataLayer
{
    public static class Bootstrapper
    {
        public static void RegisterDataLayer(this IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>();
            services.AddScoped<IMyDbContext>(s => s.GetService<MyDbContext>());
            services.AddScoped<IShowStorage, ShowStorage>();
        }
    }
}
