using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hosts.Domain.PokeApi.Contracts
{
    public class SerialiserSettings
    {
        public static JsonSerializerSettings SnakeCase = new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Formatting = Formatting.Indented
        };

    }
}