using Ozzy.Server.Queues;
using System;

namespace Ozzy.Server
{
    public class JobQueue<T> where T : class
    {
        private IQueueRepository _queueRepository;
        private QueuesFaultManager _queuesFaultManager;
        protected ISerializer Serializer;
        private TimeSpan _timeout;
        private int _retryTimes;

        public string QueueName { get; private set; }

        public JobQueue(ISerializer serializer, IQueueRepository queueRepository, QueuesFaultManager queuesFaultManager) : this(serializer, queueRepository, queuesFaultManager, null)
        {
        }
        public JobQueue(ISerializer serializer, IQueueRepository queueRepository, QueuesFaultManager queuesFaultManager, string queueName = null)
        {
            Guard.ArgumentNotNull(queueRepository, nameof(queueRepository));
            _queueRepository = queueRepository;
            _queuesFaultManager = queuesFaultManager;
            Serializer = serializer;
            if (string.IsNullOrEmpty(queueName)) queueName = this.GetType().FullName;
            QueueName = queueName;

            var sampleItem = Activator.CreateInstance(typeof(T)) as IQueueFaultSettings;
            _timeout = sampleItem?.QueueItemTimeout ?? new TimeSpan(0, 0, 10);
            _retryTimes = sampleItem?.RetryTimes ?? 5;
        }

        public void Acknowledge(string id)
        {
            _queueRepository.Acknowledge(id, QueueName);
        }

        public string Put(T item)
        {
            var payload = Serializer.BinarySerialize(item);
            return _queueRepository.Put(QueueName, payload, _retryTimes);
        }

        public QueueItem<T> Fetch()
        {
            var queueItem = _queueRepository.Fetch(QueueName, (long)_timeout.TotalSeconds);
            if (queueItem == null) return null;
            var result = Serializer.BinaryDeSerialize<T>(queueItem.Payload);
            return new QueueItem<T>(queueItem.Id, result);
        }
    }
}
