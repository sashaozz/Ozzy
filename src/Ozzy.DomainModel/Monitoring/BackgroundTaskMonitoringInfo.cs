using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.DomainModel.Monitoring
{
    public class BackgroundTaskMonitoringInfo
    {
        public string TaskName { get; set; }
        public Guid TaskId { get; set; }
        public bool IsRunning { get; set; }
        public DateTime? StartedAt { get; set; }
    }
}
