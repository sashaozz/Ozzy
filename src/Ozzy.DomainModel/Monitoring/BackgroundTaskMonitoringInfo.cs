using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.DomainModel.Monitoring
{
    public class BackgroundTaskMonitoringInfo
    {
        public string TaskName { get; set; }
        public string TaskId { get; set; }
        public string TaskState { get; set; }
        public bool IsRunning { get; set; }
        public DateTime? StartedAt { get; set; }
    }
}
