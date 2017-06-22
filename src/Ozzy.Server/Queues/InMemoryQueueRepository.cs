using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ozzy.Server
{
    //this implementation is not intended for real use. It doesn't requeue not acknowledged items.
    public class InMemoryQueueRepository : IQueueRepository
    {
        public class InMemoryQueue
        {
            public ConcurrentQueue<QueueItem> Items { get; private set; } = new ConcurrentQueue<QueueItem>();
            public ConcurrentDictionary<string, QueueItem> Fetched { get; private set; } = new ConcurrentDictionary<string, QueueItem>();
            public string QueueName { get; private set; }
            public InMemoryQueue(string queueName)
            {
                QueueName = queueName;
            }
        }

        private ConcurrentDictionary<string, InMemoryQueue> _queues = new ConcurrentDictionary<string, InMemoryQueue>();
        public ConcurrentQueue<QueueItem> DeadLetter { get; private set; } = new ConcurrentQueue<QueueItem>();

        public void Acknowledge(string id, string queueName)
        {
            var queue = _queues.GetOrAdd(queueName, name => new InMemoryQueue(name));
            queue.Fetched.TryRemove(id, out var item);
        }

        public string Put(string queueName, byte[] item, int retryCount = 5)
        {
            var id = Guid.NewGuid().ToString();
            var queue = _queues.GetOrAdd(queueName, name => new InMemoryQueue(name));
            queue.Items.Enqueue(new QueueItem(id, item)
            {
                RetryCount = retryCount,
                QueueName = queueName
            });

            return id;
        }

        public QueueItem Fetch(string queueName, long acknowledgeTimeOut = 60)
        {
            var queue = _queues.GetOrAdd(queueName, name => new InMemoryQueue(name));
            if (queue.Items.TryDequeue(out var item))
            {
                queue.Fetched.TryAdd(item.Id, item);
                item.TimeoutAt = DateTime.UtcNow.AddSeconds(acknowledgeTimeOut);
                return item;
            }
            else return null;
        }

        public List<QueueItem> GetTimeoutedItems()
        {
            return _queues
                .SelectMany(x => x.Value.Fetched)
                .Select(x => x.Value)
                .Where(i => DateTime.UtcNow > i.TimeoutAt)
                .ToList();
        }

        public void RequeueItem(QueueItem item, int retryCount = 5)
        {
            var queue = _queues.GetOrAdd(item.QueueName, name => new InMemoryQueue(name));
            if (queue.Fetched.TryRemove(item.Id, out var removedItem))
            {
                item.TimeoutAt = null;
                queue.Items.Enqueue(item);
            }
            // else throw ?
        }


        public void MoveToDeadMessageQueue(QueueItem item)
        {
            var queue = _queues.GetOrAdd(item.QueueName, name => new InMemoryQueue(name));
            queue.Fetched.TryRemove(item.Id, out var removedItem);

            DeadLetter.Enqueue(item);
        }

        public void Purge(QueueItem item)
        {
            var queue = _queues.GetOrAdd(item.QueueName, name => new InMemoryQueue(name));
            queue.Fetched.TryRemove(item.Id, out var removedItem);
        }
    }
}
