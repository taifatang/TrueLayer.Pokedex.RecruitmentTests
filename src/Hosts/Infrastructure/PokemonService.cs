using System.Linq;
using System.Threading.Tasks;
using Hosts.Domain.Models;
using Hosts.Domain.PokeApi;

namespace Hosts.Infrastructure
{
    public class PokemonService
    {
        private readonly IPokeApiHttpClient _pokeApiHttpClient;

        public PokemonService(IPokeApiHttpClient pokeApiHttpClient)
        {
            _pokeApiHttpClient = pokeApiHttpClient;
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