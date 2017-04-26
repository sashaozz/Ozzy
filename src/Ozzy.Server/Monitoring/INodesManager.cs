using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ozzy.Server
{
    public interface INodesManager
    {
        Task SaveNodeMonitoringInfo(NodeMonitoringInfo data);

        Task<List<NodeMonitoringInfo>> GetNodeMonitoringInfo();

        Task StopProcess(string nodeId, string processId);

        Task StartProcess(string nodeId, string processId);
    }
}
