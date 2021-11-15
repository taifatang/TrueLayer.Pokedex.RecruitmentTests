using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Hosts.Domain.FunTranslations;
using Hosts.Domain.FunTranslations.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hosts.Infrastructure.FunTranslations
{
    public class FunTranslationsHttpClient: IFunTranslationsHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FunTranslationsHttpClient> _logger;

        public FunTranslationsHttpClient(HttpClient httpClient, ILogger<FunTranslationsHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> Translate(string text, string language)
        {
            var response = await _httpClient.PostAsJsonAsync($"translate/{language}", new FunTranslationRequest()
            {
                Text = text
            });

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(content))
            {
                _logger.LogWarning("Received unexpected response from Fun Translation {statusCode} {content}", response.StatusCode, content);
            }

            var result =  JsonConvert.DeserializeObject<FunTranslationResponse>(content);

            return result?.Contents?.Translated;
        }
    }
}
