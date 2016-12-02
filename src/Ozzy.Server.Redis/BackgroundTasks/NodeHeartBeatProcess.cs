using System;
using System.Threading;
using System.Threading.Tasks;
using Ozzy.Core;
using StackExchange.Redis;

namespace Ozzy.Server.Redis.BackgroundProcesses
{
    public class NodeHeartBeatProcess : PeriodicAction, IBackgroundProcess
    {
        private readonly OzzyNode _node;
        private readonly IConnectionMultiplexer _redis;

        public  NodeHeartBeatProcess(OzzyNode node, RedisClient redis)
        {
            _node = node;
            _redis = redis.Redis;
        }

        public bool IsRunning => base.IsStarted;

        public string Name => this.GetType().Name;

        protected override async Task ActionAsync(CancellationTokenSource cts)
        {
            var db = _redis.GetDatabase();
            await db.HashSetAsync("key1", "nodeId", _node.NodeId);
            await db.HashSetAsync("key1", "time", DateTime.UtcNow.ToString());
        }
    }
}
