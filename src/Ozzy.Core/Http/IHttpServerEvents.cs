using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using EventSourceProxy;

namespace Ozzy.Core.Http
{
    /// <summary>
    /// Logging interface
    /// </summary>
    [EventSourceImplementation(Name = "Ozzy-Http")]
    public interface IHttpEvents : ICommonEvents
    {
        [Event(101, Level = EventLevel.Informational, Message = "{2} {0} {1}")]
        void MiddlewareHttpRequest(string method,
            string path,
            int statusCode,
            string reason,
            IDictionary<string, string[]> requestHeaders,
            IDictionary<string, string[]> responseHeaders,
            int timeTaken,
            string requestBody = null,
            string responseBody = null,
            Exception exception = null);

        [Event(102, Level = EventLevel.Verbose, Message = "Sending HttpClient Request {1} {2}")]
        void HttpClientSendRequest(Guid requestId, 
            HttpMethod method,
            Uri uri, 
            HttpRequestHeaders headers, 
            string body);

        [Event(103, Level = EventLevel.Verbose, Message = "Recieved HttpClient Response {1} {2}")]
        void HttpClientRecieveResponse(Guid requestId, 
            HttpMethod method, 
            Uri uri, 
            HttpStatusCode statusCode, 
            string reasonPhrase, 
            HttpResponseHeaders headers, 
            string body, 
            TimeSpan elapsedTime);
    }
}
