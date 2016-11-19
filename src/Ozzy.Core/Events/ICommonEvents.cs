using System;
using System.Diagnostics.Tracing;
using EventSourceProxy;

namespace Ozzy.Core
{
    [EventSourceImplementation(Name = "Ozzy-CommonEvents")]
    public interface ICommonEvents
    {
        [Event(1, Level = EventLevel.Error, Message = "{1}")]
        void Exception(Exception exception, string message = "Unhandled exception occured");

        [Event(2, Level = EventLevel.Verbose, Message = "Tracing Event : {0}")]
        void TraceVerboseEvent(string eventMessage);

        [Event(3, Level = EventLevel.Informational, Message = "Tracing Event : {0}")]
        void TraceInformationalEvent(string eventMessage);

        [Event(4, Level = EventLevel.Error, Message = "Tracing Event : {0}")]
        void TraceErrorEvent(string eventMessage);

        [Event(5, Level = EventLevel.Critical, Message = "Tracing Event : {0}")]
        void TraceCriticalEvent(string eventMessage);

        [Event(6, Level = EventLevel.Verbose, Message = "Handling {0} request")]
        void MediatrRequestCompleted(string requestType, string responseType, object request, object response, long timeElapsed);

        [Event(7, Level = EventLevel.Error, Message = "Tracing Event : {0}")]
        void MediatrRequestError(string requestType, string responseType, object request, long timeElapsed, Exception exception);

    }
}
