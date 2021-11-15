using System.Threading.Tasks;
using Hosts.Domain.PokeApi.Contracts;

namespace Hosts.Domain.PokeApi
{
    public interface IPokeApiHttpClient
    {
        Task<PokeApiSearchResponse> SearchBySpeciesAsync(string pokemon);
    }
}