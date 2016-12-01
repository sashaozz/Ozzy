using StackExchange.Redis;
using System;
using Xunit;

namespace Ozzy.Server.Tests
{
    public class DistributedLockTests
    {
        [Fact]
        public void RedisDistributedLockTest()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            var redisDistributedLockService = new RedisDistributedLockService(redis);
            var lockId = Guid.NewGuid().ToString();

            var lock1 = redisDistributedLockService.CreateLock(lockId, TimeSpan.FromMinutes(1));
            Assert.True(lock1.IsAcquired);
            var lock2 = redisDistributedLockService.CreateLock(lockId, TimeSpan.FromMinutes(1));
            Assert.False(lock2.IsAcquired);
        }
    }
}


