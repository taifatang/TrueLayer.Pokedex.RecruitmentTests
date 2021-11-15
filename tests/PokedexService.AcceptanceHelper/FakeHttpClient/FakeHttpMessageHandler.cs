using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PokedexService.AcceptanceHelper.FakeHttpClient
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public Queue<HttpRequestMessage> Requests { get; }
        public Queue<HttpResponseMessage> Responses { get; }

        public FakeHttpMessageHandler()
        {
            Requests = new Queue<HttpRequestMessage>();
            Responses = new Queue<HttpResponseMessage>();
        }

        public void QueueNextResponse(Action<HttpResponseMessage> customisations)
        {
            var response = new HttpResponseMessage();

            customisations(response);

            Responses.Enqueue(response);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Enqueue(request);

            return Task.FromResult(Responses.Dequeue());
        }
    }
}