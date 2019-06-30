using AutoMapper;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using WetherApp.Models;
using WetherApp.Repository;
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

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CityDto, City>();
            });

            IMapper mapper = config.CreateMapper();
            container.RegisterInstance(mapper);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}