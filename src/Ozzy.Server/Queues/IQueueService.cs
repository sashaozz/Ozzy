namespace Ozzy.Server
{
    public interface IQueueService<T> where T: class
    {
        void Add(T item);
        QueueItem<T> FetchNext();
        void Acknowledge(QueueItem<T> item);
    }
}
