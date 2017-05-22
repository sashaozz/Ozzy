using System;

namespace Ozzy.DomainModel
{
    public class DoNothingFaultHandler : IDomainEventsFaultHandler
    {
        public void Handle(Type processorType, IDomainEventRecord record, int retryMaxCount = 5, bool sendToErrorQueue = true)
        {
            //noop
        }
    }
}
