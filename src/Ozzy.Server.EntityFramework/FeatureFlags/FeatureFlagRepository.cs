using Ozzy.Server.EntityFramework;
using System;
using Microsoft.EntityFrameworkCore;

namespace Ozzy.Server.FeatureFlags
{

    public class FeatureFlagRepository : EfDataRepository<FeatureFlagRecord, string>, IFeatureFlagRepository
    {
        public FeatureFlagRepository(Func<AggregateDbContext> dbFactory, Func<AggregateDbContext, DbSet<FeatureFlagRecord>> dbSetProvider) 
            : base(dbFactory, dbSetProvider)
        {
        }
    }
}
