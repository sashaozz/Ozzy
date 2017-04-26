using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ozzy.Server
{
    public interface IMonitoringRepository
    {
        Task SaveNodeMonitoringInfo(NodeMonitoringInfo data);

        Task<List<NodeMonitoringInfo>> GetNodeMonitoringInfo();
    }
}
