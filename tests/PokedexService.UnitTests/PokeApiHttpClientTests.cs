using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Hosts.Domain.Exceptions;
using Hosts.Domain.PokeApi.Contracts;
using Hosts.Infrastructure.PokeApi;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using NUnit.Framework;
using PokedexService.AcceptanceHelper.FakeHttpClient;

namespace PokedexService.UnitTests
{
    [TestFixture]
    public class PokeApiHttpClientTests
    {
        private FakeHttpClient _fakeHttpClient;
        private PokeApiHttpClient _pokeApiHttpClient;

        [SetUp]
        public void SetUp()
        {
            _fakeHttpClient = FakeHttpClientFactory.Create(new Uri("https://pokeapi.local"));

            _pokeApiHttpClient = new PokeApiHttpClient(_fakeHttpClient, NullLogger<PokeApiHttpClient>.Instance);
        }

        [Test]
        public async Task Search_pokemon()
        {
            var response = new PokeApiSearchResponse()
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
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.OK;
                r.Content = new StringContent(JsonConvert.SerializeObject(response, SerialiserSettings.SnakeCase));
            });

            var result = await _pokeApiHttpClient.SearchBySpeciesAsync("mewTwo");

            result.Should().BeEquivalentTo(response);
        }

        [Test]
        public async Task Search_pokemon_using_correct_endpoint()
        {
            var pokemonName = "mewTwo";
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.OK;
                r.Content = new StringContent(JsonConvert.SerializeObject(new PokeApiSearchResponse()
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
                }));
            });

             await _pokeApiHttpClient.SearchBySpeciesAsync(pokemonName);

             _fakeHttpClient.LastRequest.RequestUri.Should().Be($"https://pokeapi.local/api/v2/pokemon-species/{pokemonName}");
        }

        [Test]
        public void Search_pokemon_unsuccessful()
        {
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.BadRequest;
            });

            Assert.ThrowsAsync<UnexpectedResponseException>(() => _pokeApiHttpClient.SearchBySpeciesAsync("mewThree"));
        }

        [Test]
        public void Search_pokemon_returns_no_content()
        {
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.OK;
            });

            Assert.ThrowsAsync<UnexpectedResponseException>(() => _pokeApiHttpClient.SearchBySpeciesAsync("SomethingIsSeriouslyBroken"));
        }
    }
}