namespace Ozzy.Server
{
    public interface IQueueRepository
    {
        //IQueryable<IQueueRecord> Query();
        void Create(QueueRecord item);
        QueueRecord FetchNext(string queueName);
        void Acknowledge(string id);
    }
    public enum QueueStatus
    {
        Awaiting = 0,
        Processing = 1
    }
}
