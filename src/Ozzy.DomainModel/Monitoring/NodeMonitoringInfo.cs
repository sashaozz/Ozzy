using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.DomainModel.Monitoring
{
    public class NodeMonitoringInfo
    {
        public string NodeId { get; set; }
        public string MachineName { get; set; }
        public DateTime NodeStartedAt { get; set; }

        public List<BackgroundTaskMonitoringInfo> BackgroundTasks { get; set; }
    }
}
