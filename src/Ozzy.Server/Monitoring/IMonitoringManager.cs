using Ozzy.DomainModel.Monitoring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.Server.Monitoring
{
    public interface IMonitoringManager
    {
        Task SaveNodeMonitoringInfo(NodeMonitoringInfo data);

        Task<List<NodeMonitoringInfo>> GetNodeMonitoringInfo();
    }
}
