using Ozzy.Server.EntityFramework;
using System;
using Microsoft.EntityFrameworkCore;

namespace Ozzy.Server.FeatureFlags
{

    public class FeatureFlagRepository : EfDataRepository<FeatureFlagRecord, string>, IFeatureFlagRepository
    {
        public FeatureFlagRepository(TransientAggregateDbContext db, Func<AggregateDbContext, DbSet<FeatureFlagRecord>> dbSetProvider) : base(db, dbSetProvider)
        {
        }
    }
}
