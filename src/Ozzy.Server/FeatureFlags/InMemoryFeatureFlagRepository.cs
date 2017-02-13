using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Ozzy.Server.FeatureFlags
{
    public class InMemoryFeatureFlagRepository: IFeatureFlagRepository
    {
        private ConcurrentDictionary<string, FeatureFlagRecord> _store = new ConcurrentDictionary<string, FeatureFlagRecord>();

        public void Create(FeatureFlagRecord item)
        {
            _store.GetOrAdd(item.Id, item);
        }       
        public IQueryable<FeatureFlagRecord> Query()
        {
            return _store.Values.AsQueryable();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public void Remove(FeatureFlagRecord item)
        {
            throw new NotImplementedException();
        }

        public void Update(FeatureFlagRecord item)
        {
            throw new NotImplementedException();
        }
    }
}
