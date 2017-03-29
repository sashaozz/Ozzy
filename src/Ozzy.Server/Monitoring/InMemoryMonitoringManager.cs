using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ozzy.DomainModel.Monitoring;

namespace Ozzy.Server.Monitoring
{
    public class InMemoryMonitoringManager : IMonitoringManager
    {
        private static Dictionary<string, NodeMonitoringInfo> _data = new Dictionary<string, NodeMonitoringInfo>();

        public void SaveNodeMonitoringInfo(NodeMonitoringInfo data)
        {
            _data[data.NodeId] = data;
        }

        public List<NodeMonitoringInfo> GetNodeMonitoringInfo()
        {
            return _data.Values.ToList();
        }
    }
}
