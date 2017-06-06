using Ozzy.Core;

namespace Ozzy.Server
{
    public class QueueItem
    {
        public QueueItem(string id, byte[] payload)
        {
            Guard.ArgumentNotNullOrEmptyString(id, nameof(id));
            Guard.ArgumentNotNull(payload, nameof(payload));
            Id = id;
            Payload = payload;
        }
        public string Id { get; private set; }
        public byte[] Payload { get; private set; }
    }
    public class QueueItem<T>
    {
        public QueueItem(string id, T item)
        {
            Guard.ArgumentNotNullOrEmptyString(id, nameof(id));
            Guard.ArgumentNotNull(item, nameof(item));
            Id = id;
            Item = item;
        }
        public string Id { get; set; }
        public T Item { get; set; }
    }
}
