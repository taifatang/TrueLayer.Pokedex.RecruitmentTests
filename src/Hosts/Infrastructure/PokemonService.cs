using System.Linq;
using System.Threading.Tasks;
using Hosts.Domain.Models;
using Hosts.Domain.PokeApi;

namespace Hosts.Infrastructure
{
    public class PokemonService
    {
        private readonly IPokeApiHttpClient _pokeApiHttpClient;
        private readonly TranslationService _translationService;

        public PokemonService(IPokeApiHttpClient pokeApiHttpClient, TranslationService translationService)
        {
            _pokeApiHttpClient = pokeApiHttpClient;
            _translationService = translationService;
        }

        public async Task<Pokemon> SearchAsync(string name)
        {
            var pokemonSearchResult = await _pokeApiHttpClient.SearchBySpeciesAsync(name);

            return new Pokemon
            {
                Name = pokemonSearchResult.Name,
                Description = pokemonSearchResult.FlavorTextEntries.First().FlavorText,
                Habitat = pokemonSearchResult.Habitat.Name,
                IsLegendary = pokemonSearchResult.IsLegendary
            };
        }
    }
}
