using System;
using System.Threading;
using System.Threading.Tasks;
using Ozzy.Core;
using Ozzy.Server.BackgroundProcesses;
using StackExchange.Redis;

namespace Ozzy.Server.Redis.BackgroundProcesses
{
    public class NodeHeartBeatProcess : PeriodicActionProcess
    {
        private readonly OzzyNode _node;
        private readonly IConnectionMultiplexer _redis;

        public  NodeHeartBeatProcess(OzzyNode node, RedisClient redis)
        {
            _node = node;
            _redis = redis.Redis;
        }
        protected override async Task ActionAsync(CancellationToken cts)
        {
            var db = _redis.GetDatabase();
            await db.HashSetAsync("key1", "nodeId", _node.NodeId);
            await db.HashSetAsync("key1", "time", DateTime.UtcNow.ToString());
        }
    }
}
