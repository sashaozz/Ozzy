using Ozzy.Core;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Ozzy.DomainModel
{
    public class InMemoryDomainEventsPubSub : IObservable<IDomainEventRecord>, IObserver<IDomainEventRecord>, IDisposable
    { 
        private List<IObserver<IDomainEventRecord>> _observers = new List<IObserver<IDomainEventRecord>>();
        private object _syncLock = new object();
   
        public void OnCompleted()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            foreach (var observer in _observers)
            {
                observer.OnError(error);
            }
        }

        public void OnNext(IDomainEventRecord value)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }

        public IDisposable Subscribe(IObserver<IDomainEventRecord> observer)
        {
            Guard.ArgumentNotNull(observer, nameof(observer));
            lock (_syncLock)
            {
                _observers.Add(observer);
            }
            return new Subscription(this, observer);
        }

        public void Unsubscribe(IObserver<IDomainEventRecord> observer)
        {
            Guard.ArgumentNotNull(observer, nameof(observer));
            lock (_syncLock)
            {
                _observers.Remove(observer);
            }                        
        }

        public void Dispose()
        {
            OnCompleted();
        }

        private sealed class Subscription : IDisposable
        {
            private IObserver<IDomainEventRecord> _observer;
            private InMemoryDomainEventsPubSub _subject;

            public Subscription(InMemoryDomainEventsPubSub subject, IObserver<IDomainEventRecord> observer)
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
