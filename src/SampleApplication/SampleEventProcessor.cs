using System;
using Ozzy.DomainModel;
using Ozzy.Server;
using Ozzy.Server.EntityFramework;

namespace SampleApplication
{
    public class SampleEventProcessor : DomainEventsProcessor
    {
        public SampleEventProcessor(IExtensibleOptions<SampleDbContext> options)
            : base(new SimpleChekpointManager(options.GetPersistedEventsReader()))
        {
        }
        public override bool HandleEvent(IDomainEventRecord record)
        {
            return false;
            //OzzyLogger<ICommonEvents>.Log.TraceInformationalEvent(record.ToString());
        }
    }

    public class LoggerEventHandler : IDomainEventsHandler
    {
        public bool HandleEvent(IDomainEventRecord record)
        {
            //OzzyLogger<ICommonEvents>.Log.TraceInformationalEvent(record.ToString());
            return true;
        }
    }

    public class LoggerEventsProcessor : DomainEventsProcessor
    {

        public LoggerEventsProcessor(IExtensibleOptions<SampleDbContext> options, Func<SampleDbContext> db)
            //: base(new SimpleChekpointManager(options.GetPersistedEventsReader()))
            : base(new EfCheckpointManager<SampleDbContext>(db, "xxx", 1))

        {
        }
        public override bool HandleEvent(IDomainEventRecord record)
        {
            //OzzyLogger<ICommonEvents>.Log.TraceInformationalEvent(record.ToString());
            return false;
        }
    }
}
