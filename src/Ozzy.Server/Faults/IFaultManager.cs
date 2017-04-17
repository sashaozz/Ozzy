using Ozzy.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Faults
{
    public interface IFaultManager
    {
        void Handle(Type processorType, DomainEventRecord record, int retryMaxCount = 5, bool sendToErrorQueue = true);
    }
}
