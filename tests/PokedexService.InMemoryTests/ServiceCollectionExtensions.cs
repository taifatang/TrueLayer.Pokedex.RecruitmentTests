using System;
using Hosts.Domain.FunTranslations;
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
            services.AddTransient<StubsModule>();

            services.AddSingleton<IPokeApiHttpClient>(provider => new PokeApiHttpClientStub(
                FakeHttpClientFactory.Create(new Uri("https://fakepokeapi.local")),
                new NullLogger<PokeApiHttpClientStub>()));

            services.AddSingleton<IFunTranslationsHttpClient>(provider => new FunTranslationsHttpClientStub(
                FakeHttpClientFactory.Create(new Uri("https://fakefuntranslation.local")),
                new NullLogger<FunTranslationsHttpClientStub>()));

            return services;
        }
    }
}
