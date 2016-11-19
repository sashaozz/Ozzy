using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading;

namespace Ozzy.Core.Events
{
    public sealed class EventEntrySubject : IObservable<EventWrittenEventArgs>, IObserver<EventWrittenEventArgs>, IDisposable
    {
        private readonly Func<EventWrittenEventArgs, bool> _filter;
        private readonly object _lockObject = new object();
        private volatile ReadOnlyCollection<Tuple<IObserver<EventWrittenEventArgs>, Func<EventWrittenEventArgs, bool>>> _observers = new List<Tuple<IObserver<EventWrittenEventArgs>, Func<EventWrittenEventArgs, bool>>>().AsReadOnly();
        private volatile bool _isFrozen;

        public EventEntrySubject(Func<EventWrittenEventArgs, bool> filter)
        {
            _filter = filter;
        }

        public EventEntrySubject()
        {
            _filter = null;
        }


        /// <summary>
        /// Releases all resources used by the current instance and unsubscribes all the observers.
        /// </summary>
        public void Dispose()
        {
            OnCompleted();
        }

        /// <summary>
        /// Notifies the provider that an observer is to receive notifications.
        /// </summary>
        /// <param name="observer">The object that is to receive notifications.</param>        
        /// <returns>A reference to an interface that allows observers to stop receiving notifications
        /// before the provider has finished sending them.</returns>
        public IDisposable Subscribe(IObserver<EventWrittenEventArgs> observer)
        {
            return Subscribe(observer, null);
        }

        /// <summary>
        /// Notifies the provider that an observer is to receive notifications.
        /// </summary>
        /// <param name="observer">The object that is to receive notifications.</param>
        /// <param name="filter">Filter</param>
        /// <returns>A reference to an interface that allows observers to stop receiving notifications
        /// before the provider has finished sending them.</returns>
        public IDisposable Subscribe(IObserver<EventWrittenEventArgs> observer, Func<EventWrittenEventArgs, bool> filter)
        {
            Guard.ArgumentNotNull(observer, "observer");
            if (filter == null) filter = any => true;
            lock (_lockObject)
            {
                if (!_isFrozen)
                {
                    var copy = _observers.ToList();
                    copy.Add(new Tuple<IObserver<EventWrittenEventArgs>, Func<EventWrittenEventArgs, bool>>(observer, filter));
                    _observers = copy.AsReadOnly();
                    return new Subscription(this, observer);
                }
            }

            observer.OnCompleted();
            return new EmptyDisposable();
        }

        private void Unsubscribe(IObserver<EventWrittenEventArgs> observer)
        {
            lock (_lockObject)
            {
                _observers = _observers.Where(x => !observer.Equals(x.Item1)).ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// </summary>
        public void OnCompleted()
        {
            var currentObservers = TakeObserversAndFreeze();

            if (currentObservers != null)
            {
                foreach (var currentObserver in currentObservers)
                {
                    currentObserver.Item1.OnCompleted();
                }
            }
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
            var currentObservers = TakeObserversAndFreeze();

            if (currentObservers != null)
            {
                foreach (var currentObserver in currentObservers)
                {
                    currentObserver.Item1.OnError(error);
                }
            }
        }

        /// <summary>
        /// Provides the observers with new data.
        /// </summary>
        /// <param name="value">The current notification information.</param>
        public void OnNext(EventWrittenEventArgs value)
        {
            if (_filter != null && !_filter(value)) return;
            foreach (var observer in _observers)
            {
                if (observer.Item2(value))
                {
                    observer.Item1.OnNext(value);
                }
            }
        }

        private ReadOnlyCollection<Tuple<IObserver<EventWrittenEventArgs>, Func<EventWrittenEventArgs, bool>>> TakeObserversAndFreeze()
        {
            lock (_lockObject)
            {
                if (!_isFrozen)
                {
                    _isFrozen = true;
                    var copy = _observers;
                    _observers = new List<Tuple<IObserver<EventWrittenEventArgs>, Func<EventWrittenEventArgs, bool>>>().AsReadOnly();
                    return copy;
                }

                return null;
            }
        }

        private sealed class Subscription : IDisposable
        {
            private IObserver<EventWrittenEventArgs> _observer;
            private EventEntrySubject _subject;

            public Subscription(EventEntrySubject subject, IObserver<EventWrittenEventArgs> observer)
            {
                _subject = subject;
                _observer = observer;
            }

            public void Dispose()
            {
                var current = Interlocked.Exchange(ref _observer, null);
                if (current != null)
                {
                    _subject.Unsubscribe(current);
                    _subject = null;
                }
            }
        }

        private sealed class EmptyDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
