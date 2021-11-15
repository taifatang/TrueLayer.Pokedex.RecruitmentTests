using System.Net;
using System.Threading.Tasks;
using Hosts.Contracts;
using Hosts.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hosts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonService _pokemonService;

        public PokemonController(PokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonSearchResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Search(string name)
        {
            var pokemon =  await _pokemonService.SearchAsync(name);
            
            return Ok(new PokemonSearchResponse(pokemon));
        }
    }
}