using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ozzy.DomainModel.Monitoring;
using System.Threading.Tasks;

namespace Ozzy.Server.Monitoring
{
    public class InMemoryMonitoringManager : IMonitoringManager
    {
        private static Dictionary<string, NodeMonitoringInfo> _data = new Dictionary<string, NodeMonitoringInfo>();

        public async Task SaveNodeMonitoringInfo(NodeMonitoringInfo data)
        {
            _data[data.NodeId] = data;
        }

        public async Task<List<NodeMonitoringInfo>> GetNodeMonitoringInfo()
        {
            return _data.Values.ToList();
        }
    }
}
