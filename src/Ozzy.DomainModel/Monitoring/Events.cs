using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.DomainModel.Monitoring
{
    public class Events
    {
        public class BackgroundProcessStopped : IDomainEvent
        {
            public string NodeId { get; set; }
            public string ProcessId { get; set; }
        }

        public class BackgroundProcessStarted : IDomainEvent
        {
            public string NodeId { get; set; }
            public string ProcessId { get; set; }
        }
    }
}
