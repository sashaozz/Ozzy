using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ozzy.Server
{
    public class InMemoryMonitoringRepository : IMonitoringRepository
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
