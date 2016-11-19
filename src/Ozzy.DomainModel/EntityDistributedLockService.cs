using System;
using System.Linq;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    public class EntityDistributedLockService : IDistributedLockService 
    {
        private readonly AggregateDbContext _dbContext;

        public EntityDistributedLockService(AggregateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IDistributedLock CreateLock(string name, TimeSpan timeout)
        {
            Guard.ArgumentNotNullOrEmptyString(name, nameof(name));
            try
            {
                var dlock = _dbContext.DistributedLocks.SingleOrDefault(l => l.Name == name) ??
                            new EntityDistributedLock(name);
                if (dlock.IsAcquired())
                {

                }
                else
                {
                    dlock.Acquire();
                }
                _dbContext.SaveChanges();
                return null;
            }
            catch (Exception e)
            {
                throw;
            }        
        }
    }
}
