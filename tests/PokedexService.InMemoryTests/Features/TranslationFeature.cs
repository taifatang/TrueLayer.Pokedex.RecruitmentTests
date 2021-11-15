using System.Collections.Generic;
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
        IWant = "search for pokemon with fun translation",
        SoThat = "is more fun")]
    [TestFixture]
    public class TranslationFeature : AcceptanceTestBase
    {
        private HttpResponseMessage _httpResponseMessage;
        private PokeApiSearchResponse _pokeApiSearchResponse;

        private static string _standardDescription = "I am the amazing Mew Two";
        private static string _yodaDescription = "Yoda is here";
        private static string _shakespeareDescription = "Then live, Macduff. What need I fear of thee?";

        [Test]
        public void Search_pokemon()
        {
            this.Given(_ => ThePokemonExists())
                .When(_ => _.ISearchPokemonWithTranslation())
                .Then(_ => _.AHttpStatusCodeIsReturned(HttpStatusCode.OK))
                .And(_ => _.ThePokenmonDescriptionShouldBe(_standardDescription))
                .BDDfy();
        }

        [Test]
        public void Search_pokemon_living_in_a_cave()
        {
            this.Given(_ => ThePokemonExists())
                .And(_ => ThePokemonLivesInACave())
                .And(_ => TheTranslationServiceAvailable())
                .When(_ => _.ISearchPokemonWithTranslation())
                .Then(_ => _.AHttpStatusCodeIsReturned(HttpStatusCode.OK))
                .And(_ => _.ThePokenmonDescriptionShouldBe(_yodaDescription))
                .BDDfy();
        }

        [Test]
        public void Search_legendary_pokemon()
        {
            this.Given(_ => ThePokemonExists())
                .And(_ => ThePokemonIsLegendary())
                .And(_ => TheTranslationServiceAvailable())
                .When(_ => _.ISearchPokemonWithTranslation())
                .Then(_ => _.AHttpStatusCodeIsReturned(HttpStatusCode.OK))
                .And(_ => _.ThePokenmonDescriptionShouldBe(_yodaDescription))
                .BDDfy();
        }

        [Test]
        public void Search_translated_pokemon_falls_back_to_standard_description()
        {
            this.Given(_ => ThePokemonExists())
                .And(_ => _.TheTranslationServiceIsUnavailable())
                .When(_ => _.ISearchPokemonWithTranslation())
                .Then(_ => _.AHttpStatusCodeIsReturned(HttpStatusCode.OK))
                .And(_ => _.ThePokenmonDescriptionShouldBe(_standardDescription))
                .BDDfy();
        }

        [Test]
        public void Search_unknown_pokemon_returns_not_found()
        {
            this.Given(_ => ThePokemonDoesNotExist())
                .When(_ => _.ISearchPokemonWithTranslation())
                .Then(_ => _.AHttpStatusCodeIsReturned(HttpStatusCode.NotFound))
                .And(_ => _.APokemonIsNotReturned())
                .BDDfy();
        }

        [TestCase("12345678")]
        [TestCase(@"!*()%")]
        [TestCase("mew2")]
        [TestCase("m$wTwo")]
        public void Search_with_invalid_pokemon_name_containing_non_letters(string invalidName)
        {
            this.When(_ => _.ISearchPokemonWithTranslation(invalidName))
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
                IsLegendary = false,
                FlavorTextEntries = new[]
                {
                    new Flavor { FlavorText = "I am the amazing Mew Two" }
                }
            };

            Stubs.PokeApiHttpClientStub.QueueNextResponse(_pokeApiSearchResponse);
        }

        [Given]
        private void ThePokemonIsLegendary()
        {
            _pokeApiSearchResponse.IsLegendary = true;
        }

        [Given]
        private void ThePokemonLivesInACave()
        {
            _pokeApiSearchResponse.Habitat.Name = "CaVe";
        }

        [Given]
        private void TheTranslationServiceIsUnavailable()
        {
            Stubs.FunTranslationsHttpClientStub.ThrowsOnNextResponse();
        }

        [Given]
        private void TheTranslationServiceAvailable()
        {
            //comments: Due to time contraint I have used a dictionary map here.
            //  The FakeHttpClient should be more sophisticated similar to richardszalay/mockhttp
            Stubs.FunTranslationsHttpClientStub.QueueNextResponse(new Dictionary<string, string>()
            {
                { "/translate/Shakespeare", _shakespeareDescription },
                { "/translate/Yoda", _yodaDescription },
            });
        }

        [Given]
        private void ThePokemonDoesNotExist()
        {
            Stubs.PokeApiHttpClientStub.QueueNotFoundResponse();
        }

        [When]
        private Task ISearchPokemonWithTranslation()
        {
            return ISearchPokemonWithTranslation("mewTwo");
        }

        [When]
        private async Task ISearchPokemonWithTranslation(string name)
        {
            _httpResponseMessage = await ApiClient.GetAsync($"pokemon/translated/{name}");
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
        private async Task ThePokenmonDescriptionShouldBe(string expectedDescription)
        {
            var responseBody = await _httpResponseMessage.Content.ReadAsStringAsync();
            var searchResult = JsonConvert.DeserializeObject<PokemonSearchResponse>(responseBody);

            searchResult.Description.Should().Be(expectedDescription);
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
