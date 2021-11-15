using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Hosts.Domain.FunTranslations.Contracts;
using Hosts.Infrastructure.FunTranslations;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using NUnit.Framework;
using PokedexService.AcceptanceHelper.FakeHttpClient;

namespace PokedexService.UnitTests.Infrastructure
{
    [TestFixture]
    public class FunTranslationsHttpClientTests
    {
        private FakeHttpClient _fakeHttpClient;
        private FunTranslationsHttpClient _pokeApiHttpClient;

        [SetUp]
        public void SetUp()
        {
            _fakeHttpClient = FakeHttpClientFactory.Create(new Uri("https://api.funtranslations.local"));

            _pokeApiHttpClient = new FunTranslationsHttpClient(_fakeHttpClient, NullLogger<FunTranslationsHttpClient>.Instance);
        }

        [Test]
        public async Task Translate()
        {
            var expectedTranslatedText = "Yoda text";
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.OK;
                r.Content = new StringContent(JsonConvert.SerializeObject(new FunTranslationResponse()
                {
                    Contents = new Contents()
                    {
                        Translated = expectedTranslatedText
                    }
                }));
            });

            var result = await _pokeApiHttpClient.Translate("original text", "Yoda");

            result.Should().Be(expectedTranslatedText);
            _fakeHttpClient.LastRequest.RequestUri.Should().Be("https://api.funtranslations.local/translate/Yoda");
        }

        [Test]
        public async Task Translation_fails()
        {
            _fakeHttpClient.QueueNextResponse(r =>
            {
                r.StatusCode = HttpStatusCode.InternalServerError;
            });

            var result = await _pokeApiHttpClient.Translate("original text", "Yoda");

            result.Should().BeNull();
        }
    }
}
