using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public class DomainEventsProcessor<TDomain> : DomainEventsProcessor where TDomain : IOzzyDomainModel
    {
        public IExtensibleOptions<TDomain> Options { get; protected set; }

        public DomainEventsProcessor(IExtensibleOptions<TDomain> options, ICheckpointManager checkpointManager) : base(checkpointManager)
        {
            Options = options;
        }
    }
}
