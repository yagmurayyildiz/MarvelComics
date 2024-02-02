using Autofac.Integration.Mvc;
using Autofac;
using System.Configuration;
using System.Web.Mvc;
using MarvelComics.Core.Services;
using MarvelComics.Core.Interfaces;
using MarvelComics.Core.Models;
using MarvelComics.Infrastructure;

namespace MarvelComics.WebUI
{
    public class AutofacConfig
    {
        public static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<CharacterService>().As<ICharacterService>();
            builder.RegisterType<ComicService>().As<IComicService>();
            builder.RegisterModule(new InfrastructureDIModule());
            // Get api and private key from appsettings
            var apiClientOptions = new MarvelAPIServiceOptions
            {
                ApiKey = ConfigurationManager.AppSettings["ApiKey"],
                PrivateKey = ConfigurationManager.AppSettings["PrivateKey"]
            };
            builder.RegisterInstance(apiClientOptions).AsSelf();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}