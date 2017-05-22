using Ozzy.DomainModel;

namespace Ozzy.Server
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
