using System;

namespace Hosts.Domain.Models
{
    public class Pokemon
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }

        public string Translation =>
            IsLegendary || Habitat.Equals("cave", StringComparison.OrdinalIgnoreCase)
            ? "Yoda"
            : "Shakespeare";
    }
}
