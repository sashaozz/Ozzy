using Ozzy.Core;
using Ozzy.DomainModel;
using Ozzy.Server;
using Ozzy.Server.DomainDsl;
using System;

namespace SampleApplication
{
    public class SampleEventProcessor : BaseEventsProcessor
    {
        public SampleEventProcessor(IExtensibleOptions<SampleDbContext> options) 
            : base(new SimpleChekpointManager(options.GetPersistedEventsReader()))
        {
        }
        protected override void HandleEvent(DomainEventRecord record)
        {
            //OzzyLogger<ICommonEvents>.Log.TraceInformationalEvent(record.ToString());
        }
    }
}
