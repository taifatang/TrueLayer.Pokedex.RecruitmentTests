using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Hosts.Domain.PokeApi.Contracts;
using Newtonsoft.Json;
using NUnit.Framework;
using PokedexService.AcceptanceHelper.Contracts;
using TestStack.BDDfy;

namespace PokedexService.InMemoryTests.Features
{
    [Story(AsA = "pokemon enthusiast",
        IWant = "search for pokemon",
        SoThat = "I can understand the pokemon more")]
    [TestFixture]
    public class SearchPokemonFeature : AcceptanceTestBase
    {
        private HttpResponseMessage _httpResponseMessage;
        private PokeApiSearchResponse _pokeApiSearchResponse;

        [Test]
        public void Search_pokemon()
        {
            this.Given(_ => ThePokemonExists())
                .When(_ => _.ISearchPokemon())
                .Then(_ => _.AHttpStatusCodeIsReturned(HttpStatusCode.OK))
                .And(_ => _.APokemonIsReturned())
                .BDDfy();
        }

        [Test]
        public void Search_unknown_pokemon_returns_not_found()
        {
            this.Given(_ => ThePokemonDoesNotExist())
                .When(_ => _.ISearchPokemon())
                .Then(_ => _.AHttpStatusCodeIsReturned(HttpStatusCode.NotFound))
                .And(_ => _.APokemonIsNotReturned())
                .BDDfy();
        }

        [TestCase("12345678")]
        [TestCase(@"!*()%")]
        [TestCase("mew2")]
        [TestCase("m$wTwo")]
        public void Search_with_invalid_pokemon_name_containing_non_letters_returns_bad_request(string invalidName)
        {
            this.When(_ => _.ISearchPokemon(invalidName))
                .Then(_ => _.AHttpStatusCodeIsReturned(HttpStatusCode.BadRequest))
                .BDDfy();
        }

        [Given]
        private void ThePokemonExists()
        {
            _pokeApiSearchResponse = new PokeApiSearchResponse()
            {
                Name = "mewTwo",
                Habitat = new Habitat()
                {
                    Name = "rare"
                },
                IsLegendary = true,
                FlavorTextEntries = new[]
                {
                    new Flavor { FlavorText = "I am the amazing Mew Two" }
                }
            };

            Stubs.PokeApiHttpClientStub.QueueNextResponse(_pokeApiSearchResponse);
        }

        [Given]
        private void ThePokemonDoesNotExist()
        {
            Stubs.PokeApiHttpClientStub.QueueNotFoundResponse();
        }

        [When]
        private async Task ISearchPokemon(string name)
        {
            _httpResponseMessage = await ApiClient.GetAsync($"pokemon/{name}");
        }

        [When]
        private Task ISearchPokemon()
        {
            return ISearchPokemon("mewtwo");
        }

        [Then]
        private async Task APokemonIsReturned()
        {
            var responseBody = await _httpResponseMessage.Content.ReadAsStringAsync();
            var searchResult = JsonConvert.DeserializeObject<PokemonSearchResponse>(responseBody);

            searchResult.Should().NotBeNull();
            searchResult.Name.Should().Be(_pokeApiSearchResponse.Name);
            searchResult.Habitat.Should().Be(_pokeApiSearchResponse.Habitat.Name);
            searchResult.IsLegendary.Should().Be(_pokeApiSearchResponse.IsLegendary);
            searchResult.Description.Should().Be(_pokeApiSearchResponse.FlavorTextEntries.First().FlavorText);
        }

        [Then]
        private async Task APokemonIsNotReturned()
        {
            var responseBody = await _httpResponseMessage.Content.ReadAsStringAsync();

            responseBody.Should().Match("* is not a Pokemon\"");
        }

        [Then]
        private void AHttpStatusCodeIsReturned(HttpStatusCode httpStatusCode)
        {
            _httpResponseMessage.StatusCode.Should().Be(httpStatusCode);
        }
    }
}
