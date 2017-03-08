using Ozzy.Core;
using System;

namespace Ozzy.DomainModel
{
    public class InMemoryEventReciever : IFastEventReciever, IObserver<DomainEventRecord>
    {
        private InMemoryDomainEventsPubSub _domainQueue;
        private IDisposable _subscription = null;
        private DomainEventsManager _loop;

        public InMemoryEventReciever(DomainEventsManager loop, InMemoryDomainEventsPubSub domainQueue)
        {
            Guard.ArgumentNotNull(domainQueue, nameof(domainQueue));
            Guard.ArgumentNotNull(loop, nameof(loop));
            _domainQueue = domainQueue;
            _loop = loop;
        }

        public void Dispose()
        {
            StopRecieving();
        }

        public void OnCompleted()
        {
            Dispose();
        }

        public void OnError(Exception error)
        {
            //
        }

        public void OnNext(DomainEventRecord value)
        {
            Recieve(value);
        }

        public void Recieve(DomainEventRecord message)
        {
            _loop.AddEventForProcessing(message);
        }

        public void StartRecieving()
        {
            _subscription = _domainQueue.Subscribe(this);
        }

        public void StopRecieving()
        {
            _subscription?.Dispose();
            _subscription = null;
        }
    }
}
