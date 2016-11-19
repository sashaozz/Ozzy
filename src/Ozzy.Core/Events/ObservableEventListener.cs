using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace Ozzy.Core.Events
{
    /// <summary>
    /// An <see cref="EventListener" /> that can be observed.
    /// </summary>
    /// <remarks>
    /// This class is thread-safe.
    /// </remarks>
    public sealed class ObservableEventListener : EventListener, IObservable<EventWrittenEventArgs>
    {
        private readonly EventEntrySubject _subject = new EventEntrySubject();
        private readonly object _deferredEnablePadlock = new object();
        private DeferredEnable _deferredEnables;

        /// <summary>
        /// Releases all resources used by the current instance and unsubscribes all the observers.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            _subject.Dispose();
        }

        /// <summary>
        /// Disables all events for the specified event source.
        /// </summary>
        /// <param name="eventSourceName">The name of the event source to enable events for.</param>
        /// <remarks>
        /// If the event source with the supplied name has already been created the request is processed immediately. Otherwise the request
        /// is deferred until the event source is created.
        /// </remarks>
        public void DisableEvents(string eventSourceName)
        {
            lock (_deferredEnablePadlock)
            {
                Guard.ArgumentNotNullOrEmptyString(eventSourceName, "eventSourceName");

                lock (_deferredEnablePadlock)
                {
                    foreach (var eventSource in EventSource.GetSources())
                    {
                        if (string.Equals(eventSource.Name, eventSourceName, StringComparison.Ordinal))
                        {
                            DisableEvents(eventSource);

                            break;
                        }
                    }

                    // cleanup deferred enables
                    ConsumeDeferredEnable(eventSourceName, _ => { });
                }
            }
        }

        /// <summary>
        /// Enables events for the event source with the specified name that have the specified verbosity level or lower.
        /// </summary>
        /// <param name="eventSourceName">The name of the event source to enable events for.</param>
        /// <param name="level">The level of events to enable.</param>
        /// <returns>
        ///   <see langword="false" /> if the request was deferred; otherwise, <see langword="true" />.
        /// </returns>
        /// <remarks>
        /// If the event source with the supplied name has already been created the request is processed immediately. Otherwise the request
        /// is deferred until the event source is created.
        /// </remarks>
        public bool EnableEvents(string eventSourceName, EventLevel level)
        {
            return EnableEvents(eventSourceName, level, EventKeywords.None);
        }

        /// <summary>
        /// Enables events for the specified event source that has the specified verbosity level or lower, and matching keyword flags.
        /// </summary>
        /// <param name="eventSourceName">The name of the event source to enable events for.</param>
        /// <param name="level">The level of events to enable.</param>
        /// <param name="matchAnyKeyword">The keyword flags necessary to enable the events.</param>
        /// <returns>
        ///   <see langword="false" /> if the request was deferred; otherwise, <see langword="true" />.
        /// </returns>
        /// <remarks>
        /// If the event source with the supplied name has already been created the request is processed immediately. Otherwise the request
        /// is deferred until the event source is created.
        /// </remarks>
        public bool EnableEvents(string eventSourceName, EventLevel level, EventKeywords matchAnyKeyword)
        {
            return EnableEvents(eventSourceName, level, matchAnyKeyword, null);
        }

        /// <summary>
        /// Enables events for the specified event source that has the specified verbosity level or lower, and matching keyword flags.
        /// </summary>
        /// <param name="eventSourceName">The name of the event source to enable events for.</param>
        /// <param name="level">The level of events to enable.</param>
        /// <param name="matchAnyKeyword">The keyword flags necessary to enable the events.</param>
        /// <param name="arguments">The arguments to be matched to enable the events.</param>
        /// <returns>
        ///   <see langword="false" /> if the request was deferred; otherwise, <see langword="true" />.
        /// </returns>
        /// <remarks>
        /// If the event source with the supplied name has already been created the request is processed immediately. Otherwise the request
        /// is deferred until the event source is created.
        /// </remarks>
        public bool EnableEvents(string eventSourceName, EventLevel level, EventKeywords matchAnyKeyword, IDictionary<string, string> arguments)
        {
            Guard.ArgumentNotNullOrEmptyString(eventSourceName, "eventSourceName");

            lock (_deferredEnablePadlock)
            {
                foreach (var eventSource in EventSource.GetSources())
                {
                    if (string.Equals(eventSource.Name, eventSourceName, StringComparison.Ordinal))
                    {
                        EnableEvents(eventSource, level, matchAnyKeyword, arguments);

                        return true;
                    }
                }

                // remove any previous deferred enable for the same source name and add a new one
                ConsumeDeferredEnable(eventSourceName, _ => { });
                _deferredEnables =
                    new DeferredEnable
                    {
                        EventSourceName = eventSourceName,
                        Level = level,
                        MatchAnyKeyword = matchAnyKeyword,
                        Arguments = arguments != null ? new Dictionary<string, string>(arguments) : null,
                        Next = _deferredEnables
                    };

                return false;
            }
        }

        /// <summary>
        /// Notifies the provider that an observer is to receive notifications.
        /// </summary>
        /// <param name="observer">The object that is to receive notifications.</param>
        /// <returns>A reference to an interface that allows observers to stop receiving notifications
        /// before the provider has finished sending them.</returns>
        public IDisposable Subscribe(IObserver<EventWrittenEventArgs> observer)
        {
            return _subject.Subscribe(observer);
        }

        /// <summary>
        /// Called whenever an event has been written by an event source for which the event listener has enabled events.
        /// </summary>
        /// <param name="eventData">The event arguments that describe the event.</param>
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {

            _subject.OnNext(eventData);
        }

        /// <summary>
        /// Called for all existing event sources when the event listener is created and when a new event source is attached to the listener.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        /// <remarks>
        /// The listener processes any deferred enable events requests associated to the <paramref name="eventSource"/>.
        /// </remarks>
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            base.OnEventSourceCreated(eventSource);

            lock (_deferredEnablePadlock)
            {
                ConsumeDeferredEnable(
                    eventSource.Name,
                    deferredEnable => EnableEvents(eventSource, deferredEnable.Level, deferredEnable.MatchAnyKeyword, deferredEnable.Arguments));
            }
        }

        private void ConsumeDeferredEnable(string eventSourceName, Action<DeferredEnable> action)
        {
            DeferredEnable previousEnable = null;
            for (var currentDeferredEnable = _deferredEnables; currentDeferredEnable != null; currentDeferredEnable = currentDeferredEnable.Next)
            {
                if (string.Equals(currentDeferredEnable.EventSourceName, eventSourceName, StringComparison.Ordinal))
                {
                    // consume the deferred enable
                    action(currentDeferredEnable);

                    // remove the entry
                    if (previousEnable == null)
                    {
                        _deferredEnables = currentDeferredEnable.Next;
                    }
                    else
                    {
                        previousEnable.Next = currentDeferredEnable.Next;
                    }

                    return;
                }

                previousEnable = currentDeferredEnable;
            }
        }

        private class DeferredEnable
        {
            public string EventSourceName;
            public EventLevel Level;
            public EventKeywords MatchAnyKeyword;
            public IDictionary<string, string> Arguments;
            public DeferredEnable Next;
        }
    }
}
