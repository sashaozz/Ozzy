using System;
using System.Diagnostics.Tracing;
using Ozzy.Core.Events;
using Xunit;

namespace Ozzy.Core.Tests
{
    public class EventSourceTests
    {
        [Fact]
        public void BasicEvent()
        {
            var eventHappened = false;
            var listener = new ObservableEventListener();
            listener.EnableEvents(OzzyLogger<ICommonEvents>.LogEventSource, EventLevel.Informational);
            listener.Subscribe(new SimpleEventObserver(e => eventHappened = true));
            OzzyLogger<ICommonEvents>.Log.Exception(new Exception());
            Assert.True(eventHappened);
        }


        [Fact]
        public void CheckEventLevel()
        {
            var errorEventHappened = false;
            var errorListener = new ObservableEventListener();
            errorListener.EnableEvents(Logger<ICommonEvents>.LogEventSource, EventLevel.Error);
            errorListener.Subscribe(new SimpleEventObserver(e => errorEventHappened = true));

            var informationalEventHappened = false;
            var informationalListener = new ObservableEventListener();
            informationalListener.EnableEvents(Logger<ICommonEvents>.LogEventSource, EventLevel.Informational);
            informationalListener.Subscribe(new SimpleEventObserver(e => informationalEventHappened = true));

            Logger<ICommonEvents>.Log.TraceInformationalEvent("Sample information");

            Assert.False(errorEventHappened);
            Assert.True(informationalEventHappened);
        }
    }
}
