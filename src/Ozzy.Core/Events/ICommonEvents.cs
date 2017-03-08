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

        [Event(2, Level = EventLevel.Verbose, Message = "{0}")]
        void TraceVerboseEvent(string eventMessage);

        [Event(3, Level = EventLevel.Informational, Message = "{0}")]
        void TraceInformationalEvent(string eventMessage);

        [Event(4, Level = EventLevel.Error, Message = "{0}")]
        void TraceErrorEvent(string eventMessage);

        [Event(5, Level = EventLevel.Critical, Message = "{0}")]
        void TraceCriticalEvent(string eventMessage);

    }
}
