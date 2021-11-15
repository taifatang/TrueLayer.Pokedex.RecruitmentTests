using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hosts.Contracts;
using Hosts.Domain.Exceptions;
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
        private readonly TranslationService _translationService;

        public PokemonController(PokemonService pokemonService, TranslationService translationService)
        {
            _pokemonService = pokemonService;
            this._translationService = translationService;
        }

        [HttpGet("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonSearchResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //Comment: I would prefer to create a [FromPath] binding with POCO attribute validation over inline validation
        public async Task<IActionResult> Search(
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can contain letters only")]string name)
        {
            try
            {
                var pokemon = await _pokemonService.SearchAsync(name);

                return Ok(new PokemonSearchResponse(pokemon));
            }
            catch (PokemonNotFoundException ex)
            {
                //Comment: I would prefer this to do done within a middleware or wrapped in Result<T> to reduce clutter
                return NotFound(ex.Message);
            }
        }

        [HttpGet("translated/{name}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonSearchResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchWithTranslation(
            [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can contain letters only")]string name)
        {
            try
            {
                var pokemon = await _pokemonService.SearchAsync(name);

                pokemon.Description = await _translationService.Translate(pokemon.Description, pokemon.Translation);;

                return Ok(new PokemonSearchResponse(pokemon));
            }
            catch (PokemonNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
