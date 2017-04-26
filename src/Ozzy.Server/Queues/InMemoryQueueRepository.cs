using System.Collections.Concurrent;
using System.Linq;

namespace Ozzy.Server
{
    public class InMemoryQueueRepository : IQueueRepository
    {
        private ConcurrentDictionary<string, QueueRecord> _store = new ConcurrentDictionary<string, QueueRecord>();
        private static object _syncLock = new object();

        public void Create(QueueRecord item)
        {
            _store.GetOrAdd(item.Id, item);
        }

        public QueueRecord FetchNext(string queueName)
        {
            lock (_syncLock)
            {
                var record = _store.Values
                    .Where(q => q.QueueName == queueName)
                    .OrderBy(v => v.CreatedAt)
                    .FirstOrDefault();

                record.Status = QueueStatus.Processing;

                return record;
            }
        }

        public IQueryable<QueueRecord> Query()
        {
            return _store.Values.AsQueryable();
        }


        public void Acknowledge(string id)
        {
            QueueRecord removed;
            _store.TryRemove(id, out removed);
        }
    }
}
