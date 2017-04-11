using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.DomainModel.Monitoring
{
    public class BackgroundTaskMonitoringInfo
    {
        public string TaskId { get; set; }
        public bool IsRunning { get; set; }
        public DateTime? StartedAt { get; set; }
    }
}
