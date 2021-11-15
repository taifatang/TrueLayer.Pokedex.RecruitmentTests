using System.Net.Http;
using Hosts.Domain.PokeApi;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PokedexService.InMemoryTests.Stubs;

namespace PokedexService.InMemoryTests
{
    public abstract class AcceptanceTestBase
    {
        protected HttpClient ApiClient;
        protected PokeApiHttpClientStub _pokeApiHttpClientStub;

        private CustomWebApplicationFactory _factory;
        
        [SetUp]
        public void SetUp()
        {
            _factory = new CustomWebApplicationFactory();
            ApiClient = _factory.CreateClient();
            _pokeApiHttpClientStub = _factory.Services.GetRequiredService<IPokeApiHttpClient>() as PokeApiHttpClientStub;
        }

        [TearDown]
        public void TearDown()
        {
            ApiClient?.Dispose();
            _factory?.Dispose();
        }
    }
}