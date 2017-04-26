using Ozzy.DomainModel;
using System;

namespace Ozzy.Server
{
    public interface IFaultManager
    {
        void Handle(Type processorType, IDomainEventRecord record, int retryMaxCount = 5, bool sendToErrorQueue = true);
    }
}
