using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PokedexService.InMemoryTests.Stubs;

namespace PokedexService.InMemoryTests
{
    public abstract class AcceptanceTestBase
    {
        protected HttpClient ApiClient;
        protected StubsModule  Stubs;

        private CustomWebApplicationFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new CustomWebApplicationFactory();
            ApiClient = _factory.CreateClient();
            Stubs = _factory.Services.GetRequiredService<StubsModule>();
        }

        [TearDown]
        public void TearDown()
        {
            ApiClient?.Dispose();
            _factory?.Dispose();
        }
    }
}
