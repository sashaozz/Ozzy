using Ozzy.DomainModel.Monitoring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.DomainModel.Monitoring
{
    public interface IMonitoringRepository
    {
        Task SaveNodeMonitoringInfo(NodeMonitoringInfo data);

        Task<List<NodeMonitoringInfo>> GetNodeMonitoringInfo();
    }
}
