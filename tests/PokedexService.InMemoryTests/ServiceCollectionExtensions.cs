using System;
using Hosts.Domain.PokeApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using PokedexService.AcceptanceHelper.FakeHttpClient;
using PokedexService.InMemoryTests.Stubs;

namespace PokedexService.InMemoryTests
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterStubs(this IServiceCollection services)
        {
            services.AddSingleton<IPokeApiHttpClient>(provider => new PokeApiHttpClientStub(
                FakeHttpClientFactory.Create(new Uri("https://fakehttpclient.local")),
                new NullLogger<PokeApiHttpClientStub>()));

            return services;
        }
    }
}