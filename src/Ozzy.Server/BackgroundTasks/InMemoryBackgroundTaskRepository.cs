using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Ozzy.Server.BackgroundTasks
{
    public class InMemoryBackgroundTaskRepository : IBackgroundTaskRepository
    {
        private ConcurrentDictionary<string, BackgroundTaskRecord> _store = new ConcurrentDictionary<string, BackgroundTaskRecord>();

        public void Create(BackgroundTaskRecord item)
        {
            _store.GetOrAdd(item.Id, item);
        }       
        public IQueryable<BackgroundTaskRecord> Query()
        {
            return _store.Values.AsQueryable();
        }

        public void Remove(string id)
        {
            BackgroundTaskRecord removed;
            _store.TryRemove(id, out removed);
        }

        public void Remove(BackgroundTaskRecord item)
        {
            BackgroundTaskRecord removed;
            _store.TryRemove(item.Id, out removed);
        }

        public void Update(BackgroundTaskRecord item)
        {
            throw new NotImplementedException();
        }
    }
}
