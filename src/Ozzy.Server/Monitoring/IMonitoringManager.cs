using Ozzy.DomainModel.Monitoring;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Monitoring
{
    public interface IMonitoringManager
    {
        void SaveNodeMonitoringInfo(NodeMonitoringInfo data);

        List<NodeMonitoringInfo> GetNodeMonitoringInfo();
    }
}
