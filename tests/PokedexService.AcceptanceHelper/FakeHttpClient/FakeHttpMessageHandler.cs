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
        private Queue<Action<Uri, HttpResponseMessage>> Responses { get; }

        public FakeHttpMessageHandler()
        {
            Requests = new Queue<HttpRequestMessage>();
            Responses = new Queue<Action<Uri, HttpResponseMessage>>();
        }

        public void QueueNextResponse(Action<Uri, HttpResponseMessage> response)
        {
            Responses.Enqueue(response);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Enqueue(request);

            var response = new HttpResponseMessage();

            var customisations = Responses.Dequeue();

            customisations(request.RequestUri, response);

            return Task.FromResult(response);
        }
    }
}
