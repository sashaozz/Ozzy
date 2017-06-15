using System.Collections.Generic;

namespace Ozzy.Server
{
    public interface IQueueRepository
    {       
        string Put(string queueName, byte[] item);
        QueueItem Fetch(string queueName);
        void Acknowledge(string id, string queueName);

        List<QueueItem> GetFetched(string queueName);
        void RequeueItem(string queueName, QueueItem item);
    }
}
