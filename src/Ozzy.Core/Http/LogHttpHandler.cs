using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.Core.Http
{
    public class LogHttpHandler : DelegatingHandler
    {
        public LogHttpHandler()
        {
        }
        public LogHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid();
            string requestBody = null;
            if (request.Content != null && (request.Content is StringContent || request.Content is FormUrlEncodedContent))
            {
                requestBody = await request.Content.ReadAsStringAsync();
            }
            OzzyLogger<IHttpEvents>.Log.HttpClientSendRequest(requestId, request.Method, request.RequestUri, request.Headers, requestBody);
                        
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var response = await base.SendAsync(request, cancellationToken);

            stopwatch.Stop();
            string responseBody = null;
            if (response.Content != null &&
                (response.Content is StringContent || response.Content is FormUrlEncodedContent))
            {
                responseBody = await response.Content.ReadAsStringAsync();
            }

            OzzyLogger<IHttpEvents>.Log.HttpClientRecieveResponse(requestId, response.RequestMessage.Method, response.RequestMessage.RequestUri,
                response.StatusCode, response.ReasonPhrase, response.Headers, responseBody, stopwatch.Elapsed);

            return response;
        }
    }
}
