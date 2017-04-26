using Ozzy.Server.EntityFramework;
using System;
using Microsoft.EntityFrameworkCore;

namespace Ozzy.Server.EntityFramework
{
    public class EfFeatureFlagRepository : EfDataRepository<FeatureFlag, string>, IFeatureFlagRepository
    {
        public EfFeatureFlagRepository(Func<AggregateDbContext> dbFactory, Func<AggregateDbContext, DbSet<FeatureFlag>> dbSetProvider)  
            : base(dbFactory, dbSetProvider)
        {
        }
    }
}
