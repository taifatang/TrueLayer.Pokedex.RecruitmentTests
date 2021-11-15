using System;
using System.Net.Http;

namespace PokedexService.AcceptanceHelper.FakeHttpClient
{
    public class FakeHttpClient : HttpClient
    {
        private readonly FakeHttpMessageHandler _fakeHttpMessageHandler;
        public HttpRequestMessage LastRequest => _fakeHttpMessageHandler.Requests.Peek();

        public FakeHttpClient(FakeHttpMessageHandler fakeHttpMessageHandler) : base(fakeHttpMessageHandler)
        {
            _fakeHttpMessageHandler = fakeHttpMessageHandler;
        }

        public void QueueNextResponse(Action<HttpResponseMessage> action)
        {
            _fakeHttpMessageHandler.QueueNextResponse(action);
        }
    }
}