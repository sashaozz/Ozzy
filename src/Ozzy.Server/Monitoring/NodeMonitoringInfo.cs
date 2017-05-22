using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server
{
    public class NodeMonitoringInfo
    {
        public string NodeId { get; set; }
        public string MachineName { get; set; }
        public DateTime MonitoringTimeStamp { get; set; }
        public List<BackgroundTaskMonitoringInfo> BackgroundTasks { get; set; }
    }
}
