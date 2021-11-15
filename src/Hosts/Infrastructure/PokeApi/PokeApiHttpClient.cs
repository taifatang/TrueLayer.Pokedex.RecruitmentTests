using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Hosts.Domain.Exceptions;
using Hosts.Domain.PokeApi;
using Hosts.Domain.PokeApi.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hosts.Infrastructure.PokeApi
{
    public class PokeApiHttpClient : IPokeApiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PokeApiHttpClient> _logger;

        public PokeApiHttpClient(HttpClient httpClient, ILogger<PokeApiHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PokeApiSearchResponse> SearchBySpeciesAsync(string pokemon)
        {
            var response = await _httpClient.GetAsync($"/api/v2/pokemon-species/{pokemon}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new PokemonNotFoundException(pokemon);
            }

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(content))
            {
                _logger.LogWarning("Received unexpected response from Poke Api {statusCode} {content}", response.StatusCode, content);

                throw new UnexpectedResponseException(nameof(PokeApiHttpClient));
            }

            return JsonConvert.DeserializeObject<PokeApiSearchResponse>(content, SerialiserSettings.SnakeCase);
        }
    }
}