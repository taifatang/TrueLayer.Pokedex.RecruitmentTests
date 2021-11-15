using System.Collections.Generic;

namespace Hosts.Domain.PokeApi.Contracts
{
    public class PokeApiSearchResponse
    {
        public string Name { get; set; }
        public Habitat Habitat { get; set; }
        public bool IsLegendary { get; set; }
        public IEnumerable<Flavor> FlavorTextEntries { get; set; }
    }

    public class Flavor
    {
        public string FlavorText { get; set; }
    }

    public class Habitat
    {
        public string Name { get; set; }
    }
}