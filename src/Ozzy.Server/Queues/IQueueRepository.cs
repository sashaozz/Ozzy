using System.Collections.Generic;

namespace Ozzy.Server
{
    public interface IQueueRepository
    {
        string Put(string queueName, byte[] item, int maxRetries = 5);

        QueueItem Fetch(string queueName, long acknowledgeTimeOut = 60);
        void Acknowledge(string id, string queueName);
        List<QueueItem> GetTimeoutedItems();

        void RequeueItem(QueueItem item, int retryCount = 5);
        void MoveToDeadMessageQueue(QueueItem item);
        void Purge(QueueItem item);
    }
}
