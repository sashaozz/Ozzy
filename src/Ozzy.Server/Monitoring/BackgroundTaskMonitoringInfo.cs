using System;

namespace Ozzy.Server
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
