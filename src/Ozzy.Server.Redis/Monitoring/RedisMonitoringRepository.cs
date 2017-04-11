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
    public class RedisMonitoringRepository : IMonitoringRepository
    {
        private readonly IConnectionMultiplexer _redis;
        private const string _nodesKey = "monitoring_nodes";

        public RedisMonitoringRepository(RedisClient redis)
        {
            _redis = redis.Redis;
        }

        public async Task<List<NodeMonitoringInfo>> GetNodeMonitoringInfo()
        {
            try
            {
                var db = _redis.GetDatabase();
                var values = await db.HashValuesAsync(_nodesKey);
                return values.Select(v => JsonConvert.DeserializeObject<NodeMonitoringInfo>(v)).ToList();
            }
            catch {
                return new List<NodeMonitoringInfo>();
            }
        }

        public async Task SaveNodeMonitoringInfo(NodeMonitoringInfo data)
        {
            var db = _redis.GetDatabase();
            await db.HashSetAsync(_nodesKey, data.NodeId, JsonConvert.SerializeObject(data));
        }
    }
}
