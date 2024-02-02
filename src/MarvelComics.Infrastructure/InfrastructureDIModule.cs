using Autofac;
using MarvelComics.Core.Interfaces;
using MarvelComics.Infrastructure.External_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MarvelComics.Infrastructure
{
    public class InfrastructureDIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Registeration of HttpClient as a Singleton
            builder.Register(c => new HttpClient
            {
                BaseAddress = new Uri("https://gateway.marvel.com/")
            })
            .As<HttpClient>()
            .SingleInstance();

            builder.RegisterType<MarvelAPIService>().As<IMarvelAPIService>();
        }
    }
}
