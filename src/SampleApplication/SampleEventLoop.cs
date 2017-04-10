using System;
using Ozzy.Server;

namespace SampleApplication
{
    public class SampleEventLoop : DomainEventsLoop<SampleDbContext>
    {
        public SampleEventLoop(IExtensibleOptions<SampleDbContext> options) : base(options)
        {
            
        }
    }
}
