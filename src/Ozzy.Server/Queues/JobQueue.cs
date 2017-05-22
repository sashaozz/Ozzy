namespace Ozzy.Server
{
    public class JobQueue<T> where T : class
    {
        private IQueueRepository _queueRepository;
        protected ISerializer Serializer;

        public string QueueName { get; private set; }

        public JobQueue(ISerializer serializer, IQueueRepository queueRepository) : this(serializer, queueRepository, null)
        {
        }
        public JobQueue(ISerializer serializer, IQueueRepository queueRepository, string queueName = null)
        {
            Guard.ArgumentNotNull(queueRepository, nameof(queueRepository));
            _queueRepository = queueRepository;
            Serializer = serializer;
            if (string.IsNullOrEmpty(queueName)) queueName = typeof(T).FullName;
            QueueName = queueName;
        }

        public void Acknowledge(string id)
        {
            _queueRepository.Acknowledge(id, QueueName);
        }

        public string Put(T item)
        {
            var payload = Serializer.BinarySerialize(item);
            return _queueRepository.Put(QueueName, payload);
        }

        public QueueItem<T> Fetch()
        {
            var queueItem = _queueRepository.Fetch(QueueName);
            if (queueItem == null) return null;
            var result = Serializer.BinaryDeSerialize<T>(queueItem.Payload);
            return new QueueItem<T>(queueItem.Id, result);
        }
    }
}
