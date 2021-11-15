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

namespace PokedexService.InMemoryTests.Fixtures
{
    [Story(AsA = "pokemon enthusiast",
        IWant = "search for pokemon",
        SoThat = "I can understand the pokemon more")]
    [TestFixture]
    public class SearchPokemonFixture : AcceptanceTestBase
    {
        private HttpResponseMessage _httpResponseMessage;
        private PokeApiSearchResponse _pokeApiSearchResponse;

        [Test]
        public void Search_pokemon()
        {
            this.Given(_ => APokemonExists())
                .When(_ => _.ISearchPokemon())
                .Then(_ => _.AHttpStatusCodeIsReturned(HttpStatusCode.OK))
                .And(_ => _.APokemonIsReturned())
                .BDDfy();
        }

        [Given]
        private void APokemonExists()
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
                    new Flavor {FlavorText = "I am the amazing Mew Two"}
                }
            };
            
            _pokeApiHttpClientStub.QueueNextResponse(_pokeApiSearchResponse);
        }

        [When]
        private async Task ISearchPokemon()
        {
            _httpResponseMessage = await ApiClient.GetAsync($"pokemon/mewtwo");
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
        private void AHttpStatusCodeIsReturned(HttpStatusCode httpStatusCode)
        {
            _httpResponseMessage.StatusCode.Should().Be(httpStatusCode);
        }
    }
}