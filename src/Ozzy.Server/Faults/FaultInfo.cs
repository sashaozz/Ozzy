using Ozzy.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Faults
{
    public class FaultInfo
    {
        public Type ProcessorType { get; set; }
        public DomainEventRecord Record { get; set; }
        public int RetryMaxCount { get; set; }
    }
}
