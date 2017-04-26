using Ozzy.Core;

namespace Ozzy.DomainModel
{
    public class InMemoryEventPublisher : IFastEventPublisher
    {
        private InMemoryDomainEventsPubSub _domainQueue;

        public InMemoryEventPublisher(InMemoryDomainEventsPubSub domainQueue)
        {
            Guard.ArgumentNotNull(domainQueue, nameof(domainQueue));
            _domainQueue = domainQueue;
        }

        public void Publish(IDomainEventRecord message)
        {
            _domainQueue.OnNext(message);
        }
    }
}
