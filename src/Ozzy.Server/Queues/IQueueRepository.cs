using Ozzy.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Queues
{
    public interface IQueueRepository : IDataRepository<QueueRecord, string>
    {
        QueueRecord FetchNext(string queueName);
    }
}
