using System;
using System.Diagnostics.Tracing;
using Ozzy.Core.Events;
using Xunit;

namespace Ozzy.Core.Tests
{
    public class EventSourceTests
    {
        [Fact]
        public void Test1()
        {
            var eventHappened = false;
            var listener = new ObservableEventListener();
            listener.EnableEvents(OzzyLogger<ICommonEvents>.LogEventSource, EventLevel.Informational);
            listener.Subscribe(new SimpleEventObserver(e => eventHappened = true));
            OzzyLogger<ICommonEvents>.Log.Exception(new Exception());
            Assert.True(eventHappened);
        }
    }
}
