using Hosts.Domain.Models;

namespace Hosts.Contracts
{
    public class PokemonSearchResponse
    {
        public string Name { get; }
        public string Description { get; }
        public string Habitat { get; }
        public bool IsLegendary { get; }

        public PokemonSearchResponse(Pokemon pokemon)
        {
            Name = pokemon.Name;
            Description = pokemon.Description;
            Habitat = pokemon.Habitat;
            IsLegendary = pokemon.IsLegendary;
        }
    }
}