using System;
using Ozzy.DomainModel;
using Ozzy.Server;
using Ozzy.Server.EntityFramework;

namespace SampleApplication
{
    public class SampleEventProcessor : DomainEventsProcessor<SampleDbContext>
    {
        public SampleEventProcessor(IExtensibleOptions<SampleDbContext> options)
            : base(options, new SimpleChekpointManager(options.GetPersistedEventsReader()))
        {
        }
        protected override bool HandleEvent(DomainEventRecord record)
        {
            return false;
            //OzzyLogger<ICommonEvents>.Log.TraceInformationalEvent(record.ToString());
        }
    }

    public class LoggerEventHandler : IDomainEventHandler
    {
        public bool HandleEvent(DomainEventRecord record)
        {
            //OzzyLogger<ICommonEvents>.Log.TraceInformationalEvent(record.ToString());
            return true;
        }
    }

    public class LoggerEventsProcessor : DomainEventsProcessor
    {

        public LoggerEventsProcessor(IExtensibleOptions<SampleDbContext> options, Func<SampleDbContext> db)
            //: base(new SimpleChekpointManager(options.GetPersistedEventsReader()))
            : base(new DbCheckpointManager<SampleDbContext>(db, "xxx", 1))

        {
        }
        protected override bool HandleEvent(DomainEventRecord record)
        {
            //OzzyLogger<ICommonEvents>.Log.TraceInformationalEvent(record.ToString());
            return false;
        }
    }
}
