using Ozzy.Core;
using Ozzy.DomainModel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server
{
    public class RedisDistributedLockService : IDistributedLockService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly string _nodeName;

        public RedisDistributedLockService(IConnectionMultiplexer redis, string nodeName = "node")
        {
            _redis = redis;
            _nodeName = nodeName;
        }

        public IDistributedLock CreateLock(string name, TimeSpan timeout)
        {
            Guard.ArgumentNotNullOrEmptyString(name, nameof(name));

            try
            {
                var db = _redis.GetDatabase();
                var isAcquired = db.LockTake(name, _nodeName, timeout);
                return new RedisDistributedLock(isAcquired, timeout, DateTime.UtcNow);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
