using System.Linq;
using BusinessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Middlewares;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.RegisterBusinessLayer();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }*/
            app.UseMiddleware<SmartResponseProcessor>();

            app.UseMvc();

            AutoMapper.Mapper.Initialize(ConfigureMapping);
        }

        public static void ConfigureMapping(AutoMapper.IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<BusinessLayer.Entities.SearchResults<BusinessLayer.Providers.ShowProvider.Entities.Show>, Entities.SearchResults<Entities.Show>>();

            cfg.CreateMap<BusinessLayer.Providers.ShowProvider.Entities.Show, Entities.Show>()
                .ForMember(d => d.Cast, opt => opt.MapFrom(s => s.People.OrderByDescending(c => c.Birthday).ToList()));

            cfg.CreateMap<BusinessLayer.Providers.ShowProvider.Entities.Person, Entities.Person>()
                .ForMember(d => d.Birthday, opt => opt.MapFrom(s => s.Birthday.HasValue ? s.Birthday.Value.ToString("yyyy-MM-dd") : null));

            Bootstrapper.ConfigureMapping(cfg);
        }
    }
}
