using Ozzy.Server.Monitoring;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ozzy.DomainModel.Monitoring;
using StackExchange.Redis;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ozzy.Server.Redis.Monitoring
{
    public class RedisMoniitoringManager : IMonitoringManager
    {
        private readonly IConnectionMultiplexer _redis;
        private const string _nodesKey = "monitoring_nodes";

        public RedisMoniitoringManager(RedisClient redis)
        {
            _redis = redis.Redis;
        }

        public async Task<List<NodeMonitoringInfo>> GetNodeMonitoringInfo()
        {
            var db = _redis.GetDatabase();
            var values = await db.HashValuesAsync(_nodesKey);
            return values.Select(v => JsonConvert.DeserializeObject<NodeMonitoringInfo>(v)).ToList();
        }

        public async Task SaveNodeMonitoringInfo(NodeMonitoringInfo data)
        {
            var db = _redis.GetDatabase();
            await db.HashSetAsync(_nodesKey, data.NodeId, JsonConvert.SerializeObject(data));
        }
    }
}
