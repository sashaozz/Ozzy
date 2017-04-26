using System;
using System.Diagnostics.Tracing;
using EventSourceProxy;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    [EventSourceImplementation(Name = "Ozzy-DomainModel")]
    public interface IDomainModelTracing : ICommonEvents
    {
        [Event(101, Level = EventLevel.Verbose, Message = "Publishing new record to fast channel")]
        void TracePublishToFastChannel(IDomainEventRecord record);
        [Event(102, Level = EventLevel.Warning, Message = "Error during publishing new record to fast channel")]
        void PublishToFastChannelException(Exception e);
        [Event(103, Level = EventLevel.Verbose, Message = "Processing domain event entry")]
        void ProcessDomainEventEntry(DomainEventEntry entry);
        [Event(104, Level = EventLevel.Error, Message = "Error processing domain event entry")]
        void ProcessDomainEventEntryException(DomainEventEntry entry, long sequence, Exception exception);
        [Event(105, Level = EventLevel.Error, Message = "Error starting domain events processor")]
        void EventsProcessorStartException(Exception exception);
        [Event(106, Level = EventLevel.Error, Message = "Error stopping domain events processor")]
        void EventsProcessorStopException(Exception exception);
        [Event(107, Level = EventLevel.Verbose, Message = "Polled {0} events from store")]
        void Polling(int count);
        [Event(108, Level = EventLevel.Error, Message = "Exception during polling events from from durable store")]
        void PollException(Exception exception);
        [Event(109, Level = EventLevel.Error, Message = "Exception during polling events from from durable store : {0}")]
        void ProcessPollEvent(Exception exception);
        [Event(110, Level = EventLevel.Error, Message = "Exception during processing polled event")]
        void ProcessPollEventException(Exception exception);
    }    
}
