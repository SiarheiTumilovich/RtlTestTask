using System.Linq;
using System.Runtime.CompilerServices;
using AutoMapper;
using BusinessLayer.Providers.AppSettings;
using BusinessLayer.Providers.ShowGrabbing;
using BusinessLayer.Providers.ShowGrabbing.Grabbers;
using BusinessLayer.Providers.ShowProvider;
using BusinessLayer.Providers.TvMaze;
using DataLayer;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("BusinessLayer.Tests")]
[assembly: InternalsVisibleTo("WebApi.IntegrationTests")]

namespace BusinessLayer
{
    public static class Bootstrapper
    {
        public static void RegisterBusinessLayer(this IServiceCollection services)
        {
            services.RegisterDataLayer();
            services.AddScoped<IShowProvider, ShowProvider>();
            services.AddScoped<IShowGrabberFactory, ShowGrabberFactory>();
            services.AddScoped<TvMazeShowGrabber>();
            services.AddScoped<ITvMazeApi, TvMazeApi>();
            services.AddScoped<IAppSettingsProvider, AppSettingsProvider>();
        }

        public static void ConfigureMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<DataLayer.Domains.Show, Providers.ShowProvider.Entities.Show>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ShowId))
                .ForMember(d => d.People, opt => opt.MapFrom(s => s.People.Select(a => a.Person)))
                .ReverseMap()
                .ForMember(d => d.People, opt => opt.ResolveUsing((s, d, o, c) =>
                {
                    return s.People.Select(p => new DataLayer.Domains.ShowPersonAssoc
                    {
                        ShowId = s.Id,
                        PersonId = p.Id,
                        Person = c.Mapper.Map<DataLayer.Domains.Person>(p)
                    });
                }));

            cfg.CreateMap<DataLayer.Domains.Person, Providers.ShowProvider.Entities.Person>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.PersonId))
                .ReverseMap();

            cfg.CreateMap<Providers.TvMaze.Entities.TvMazeShow, Providers.ShowProvider.Entities.Show>();
            cfg.CreateMap<Providers.TvMaze.Entities.TvMazeCast, Providers.ShowProvider.Entities.Person>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Person.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Person.Name))
                .ForMember(d => d.Birthday, opt => opt.MapFrom(s => s.Person.Birthday));
            cfg.CreateMap<Providers.TvMaze.Entities.TvMazeCast.TvMazeCastPerson, Providers.ShowProvider.Entities.Person>();
        }
    }
}
