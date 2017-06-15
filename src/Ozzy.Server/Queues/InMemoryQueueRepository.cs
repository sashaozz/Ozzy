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

        public void Acknowledge(string id, string queueName)
        {
            var queue = _queues.GetOrAdd(queueName, name => new InMemoryQueue(name));
            queue.Fetched.TryRemove(id, out var item);
        }

        public string Put(string queueName, byte[] item)
        {
            var id = Guid.NewGuid().ToString();
            var queue = _queues.GetOrAdd(queueName, name => new InMemoryQueue(name));
            queue.Items.Enqueue(new QueueItem(id, item));
            return id;
        }

        public QueueItem Fetch(string queueName)
        {
            var queue = _queues.GetOrAdd(queueName, name => new InMemoryQueue(name));
            if (queue.Items.TryDequeue(out var item))
            {
                queue.Fetched.TryAdd(item.Id, item);
                item.FetchedAt = DateTime.Now;
                return item;
            }
            else return null;
        }

        public List<QueueItem> GetFetched(string queueName)
        {
            return _queues[queueName]?.Fetched.Select(x => x.Value).ToList() ?? new List<QueueItem>();
        }

        public void RequeueItem(string queueName, QueueItem item)
        {
            var queue = _queues.GetOrAdd(queueName, name => new InMemoryQueue(name));
            if (queue.Fetched.TryRemove(item.Id, out var removedItem))
            {
                queue.Items.Enqueue(item);
            }
            // else throw ?
        }
    }
}
