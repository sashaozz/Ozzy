using Microsoft.EntityFrameworkCore;
using Ozzy.DomainModel;

namespace Ozzy.Server.EntityFramework
{

    public class TransientAggregateDbContext : AggregateDbContext
    {
        public TransientAggregateDbContext(DbContextOptions<AggregateDbContext> options) : base(options)
        {
        }
        public TransientAggregateDbContext(DbContextOptions<AggregateDbContext> options, IFastEventPublisher publisher) : base(options, publisher)
        {
        }

        public override AggregateDbContext Clone()
        {
            return new TransientAggregateDbContext(_options, FastEventPublisher);
        }
    }
}
