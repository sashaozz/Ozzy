using Ozzy.DomainModel;
using Ozzy.Server;
using Ozzy.Server.Configuration;

namespace SampleApplication
{
    public class SampleEventLoop : DomainEventLoop<SampleDbContext>
    {
        public SampleEventLoop(IExtensibleOptions<SampleDbContext> options) : base(options)
        {
            AddHandler(new SampleEventProcessor(options));
        }
    }
}
