using Ozzy.DomainModel;
using System;

namespace Ozzy.Server
{
    public class FaultInfo
    {
        public Type ProcessorType { get; set; }
        public IDomainEventRecord Record { get; set; }
        public int RetryMaxCount { get; set; }
    }
}
