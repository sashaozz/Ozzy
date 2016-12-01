using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ozzy.Server.Tests
{
    public class RedisLockServiceTest
    {
        [Fact]
        public void RedisDistributedLockServiceTest()
        {
            var redis = ConnectionMultiplexer.Connect("localhost");
            var redisDistributedLockService = new RedisDistributedLockService(redis);

            var lock1 = redisDistributedLockService.CreateLock("Test", TimeSpan.FromMinutes(1));
            var lock2 = redisDistributedLockService.CreateLock("Test", TimeSpan.FromMinutes(1));
        }
    }
}
