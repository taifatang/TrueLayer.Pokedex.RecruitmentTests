using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using PokedexService.AcceptanceHelper.Contracts;
using TestStack.BDDfy;

namespace PokedexService.AcceptanceTests.Features
{
    [Story(AsA = "pokemon enthusiast",
        IWant = "search for pokemon",
        SoThat = "I can understand the pokemon more")]
    [TestFixture]
    public class SearchPokemonFixture
    {
        private HttpClient _apiClient;

        private string _pokemon;
        private HttpResponseMessage _httpResponseMessage;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            _apiClient = new HttpClient()
            {
                BaseAddress = new Uri(builder["Service:BaseUrl"])
            };
        }

        [Test]
        public void Search_pokemon()
        {
            this.Given(_ => _.IAmLookingFor("mewtwo"))
                .When(_ => _.ISearchPokemon())
                .Then(_ => _.AHttpStatusCodeIsReturned(HttpStatusCode.OK))
                .And(_ => _.APokemonIsReturned())
                .BDDfy();
        }

        [Given]
        private void IAmLookingFor(string pokemon)
        {
            _pokemon = pokemon;
        }

        [When]
        private async Task ISearchPokemon()
        {
            _httpResponseMessage = await _apiClient.GetAsync($"pokemon/{_pokemon}");
        }

        [Then]
        private async Task APokemonIsReturned()
        {
            var responseBody = await _httpResponseMessage.Content.ReadAsStringAsync();
            var searchResult = JsonConvert.DeserializeObject<PokemonSearchResponse>(responseBody);

            searchResult.Should().NotBeNull();
            searchResult.Name.Should().Be(_pokemon);
        }

        [Then]
        private void AHttpStatusCodeIsReturned(HttpStatusCode httpStatusCode)
        {
            _httpResponseMessage.StatusCode.Should().Be(httpStatusCode);
        }
    }
}
