using System.Net;
using System.Net.Http;
using Hosts.Domain.PokeApi.Contracts;
using Hosts.Infrastructure.PokeApi;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokedexService.AcceptanceHelper.FakeHttpClient;

namespace PokedexService.InMemoryTests.Stubs
{
    public class PokeApiHttpClientStub : PokeApiHttpClient
    {
        private readonly FakeHttpClient _fakeHttpClient;

        public PokeApiHttpClientStub(FakeHttpClient httpClient, ILogger<PokeApiHttpClientStub> logger) : base(
            httpClient, logger)
        {
            _fakeHttpClient = httpClient;
        }

        public void QueueNextResponse(PokeApiSearchResponse searchResponse)
        {
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.OK;
                r.Content = new StringContent(JsonConvert.SerializeObject(searchResponse, SerialiserSettings.SnakeCase));
            });
        }
        
        public void QueueNotFoundResponse()
        {
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.NotFound;
                r.Content = new StringContent(string.Empty);
            });
        }
    }
}