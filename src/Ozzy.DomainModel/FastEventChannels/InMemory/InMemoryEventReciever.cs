using Ozzy.Core;
using System;

namespace Ozzy.DomainModel
{
    public class InMemoryEventReciever : IFastEventReciever, IObserver<IDomainEventRecord>
    {
        private InMemoryDomainEventsPubSub _domainQueue;
        private IDisposable _subscription = null;
        private DomainEventsLoop _loop;

        public InMemoryEventReciever(DomainEventsLoop loop, InMemoryDomainEventsPubSub domainQueue)
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

        public void OnNext(IDomainEventRecord value)
        {
            Recieve(value);
        }

        public void Recieve(IDomainEventRecord message)
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
