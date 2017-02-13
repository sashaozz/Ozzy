using Ozzy.Server;
using Ozzy.Server.EntityFramework;

namespace Ozzy.Core.Tests.Fixtures
{
    public class TestDomainContext : AggregateDbContext
    {
        public TestDomainContext(OzzyDomainOptions<TestDomainContext> options) 
            :base(options)
        {
        }
    }
}
