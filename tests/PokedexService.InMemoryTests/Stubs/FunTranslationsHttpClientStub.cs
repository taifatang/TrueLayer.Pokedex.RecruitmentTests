using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Hosts.Domain.FunTranslations.Contracts;
using Hosts.Infrastructure.FunTranslations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PokedexService.AcceptanceHelper.FakeHttpClient;

namespace PokedexService.InMemoryTests.Stubs
{
    public class FunTranslationsHttpClientStub : FunTranslationsHttpClient
    {
        private readonly FakeHttpClient _fakeHttpClient;

        public FunTranslationsHttpClientStub(FakeHttpClient httpClient, ILogger<FunTranslationsHttpClientStub> logger) : base(
            httpClient, logger)
        {
            _fakeHttpClient = httpClient;
        }

        public void QueueNextResponse(Dictionary<string, string> responseMap)
        {
            _fakeHttpClient.QueueNextResponse((path,r) =>
            {
                r.StatusCode = HttpStatusCode.OK;
                r.Content = new StringContent(JsonConvert.SerializeObject(new FunTranslationResponse
                {
                    Contents = new Contents()
                    {
                        Translated = responseMap[path.AbsolutePath]
                    }
                }));
            });
        }

        public void ThrowsOnNextResponse()
        {
            _fakeHttpClient.QueueNextResponse(r => throw new TimeoutException());
        }
    }
}
