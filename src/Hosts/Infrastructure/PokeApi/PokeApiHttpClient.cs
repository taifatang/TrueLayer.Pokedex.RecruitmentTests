using System.Net.Http;
using System.Threading.Tasks;
using Hosts.Domain.Exceptions;
using Hosts.Domain.PokeApi;
using Hosts.Domain.PokeApi.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hosts.Infrastructure.PokeApi
{
    public class PokeApiHttpClient: IPokeApiHttpClient
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

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Received unexpected response from Poke Api {statusCode} {content}", response.StatusCode, content);

                throw new UnexpectedResponseException(nameof(PokeApiHttpClient));
            }

            if (!string.IsNullOrEmpty(content))
            {
                return JsonConvert.DeserializeObject<PokeApiSearchResponse>(content, SerialiserSettings.SnakeCase);
            }

            _logger.LogWarning("Received successful response from Poke Api {statusCode} but unexpected empty content", response.StatusCode);

            throw new UnexpectedResponseException(nameof(PokeApiHttpClient));
        }
    }
}