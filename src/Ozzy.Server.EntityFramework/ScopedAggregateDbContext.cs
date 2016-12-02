using Microsoft.EntityFrameworkCore;
using Ozzy.DomainModel;

namespace Ozzy.Server.EntityFramework
{
    public class ScopedAggregateDbContext : AggregateDbContext
    {
        public ScopedAggregateDbContext(DbContextOptions<AggregateDbContext> options) : base(options)
        {
        }
        public ScopedAggregateDbContext(DbContextOptions<AggregateDbContext> options, IFastEventPublisher publisher) : base(options, publisher)
        {
        }
    }
}
