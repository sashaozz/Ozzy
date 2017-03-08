using Ozzy.Core;
using Ozzy.Server;
using Ozzy.Server.EntityFramework;

namespace SampleApplication
{
    public class SampleDbContext : AggregateDbContext
    {
        public SampleDbContext(IExtensibleOptions<SampleDbContext> options) : base(options)
        {            
        }
    }
}
