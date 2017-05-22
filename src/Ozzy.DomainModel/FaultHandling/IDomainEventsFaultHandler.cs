using System;

namespace Ozzy.DomainModel
{
    public interface IDomainEventsFaultHandler
    {
        void Handle(Type processorType, IDomainEventRecord record, int retryMaxCount = 5, bool sendToErrorQueue = true);
    }
}
