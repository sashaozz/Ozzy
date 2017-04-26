using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Ozzy.Server
{
    public class InMemoryFeatureFlagRepository: IFeatureFlagRepository
    {
        private ConcurrentDictionary<string, FeatureFlag> _store = new ConcurrentDictionary<string, FeatureFlag>();

        public void Create(FeatureFlag item)
        {
            _store.GetOrAdd(item.Id, item);
        }       
        public IQueryable<FeatureFlag> Query()
        {
            return _store.Values.AsQueryable();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public void Remove(FeatureFlag item)
        {
            throw new NotImplementedException();
        }

        public void Update(FeatureFlag item)
        {
            throw new NotImplementedException();
        }
    }
}
