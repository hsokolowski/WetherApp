using AutoMapper;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using WetherApp.Models;
using WetherApp.Repository;
using WetherApp.RepositoryApi;
using WetherApp.ViewModel;

namespace WetherApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ICityRepository, CityRepository>();
            container.RegisterType<IApiRepository, ApiRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<City, CityDto>()
                    .ForMember(x =>x.localization, opt => opt.MapFrom(src =>"("+src.country+") "+ src.name));
            });

            IMapper mapper = config.CreateMapper();
            container.RegisterInstance(mapper);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}