namespace Ozzy.Server
{
    public class QueueItem<T>
    {
        public string QueueId { get; set; }
        public T Item { get; set; }
    }
}
