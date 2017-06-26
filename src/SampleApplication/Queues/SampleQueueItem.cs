using Ozzy.Server.Queues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.Queues
{
    public class SampleQueueItem: IQueueFaultSettings
    {
        public string Field1 { get; set; }
        public int Field2 { get; set; }

        public TimeSpan QueueItemTimeout => new TimeSpan(0,0,5);

        public int RetryTimes => 3;
    }
}
