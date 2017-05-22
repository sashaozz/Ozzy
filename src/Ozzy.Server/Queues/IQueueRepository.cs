namespace Ozzy.Server
{
    public interface IQueueRepository
    {       
        string Put(string queueName, byte[] item);
        QueueItem Fetch(string queueName);
        void Acknowledge(string id, string queueName);
    }
}
