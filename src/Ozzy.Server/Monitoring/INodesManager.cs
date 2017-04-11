using Ozzy.DomainModel.Monitoring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.Server.Monitoring
{
    public interface INodesManager
    {
        Task SaveNodeMonitoringInfo(NodeMonitoringInfo data);

        Task<List<NodeMonitoringInfo>> GetNodeMonitoringInfo();

        Task StopProcess(string nodeId, Guid processId);

        Task StartProcess(string nodeId, Guid processId);
    }
}
