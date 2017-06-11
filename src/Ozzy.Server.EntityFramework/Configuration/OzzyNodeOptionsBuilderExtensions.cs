using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ozzy.DomainModel;
using Ozzy.Server.EntityFramework;


namespace Ozzy.Server.Configuration
{
    public static class OzzyNodeOptionsBuilderExtensions
    {
        public static OzzyNodeOptionsBuilder<TDomain> UseEFDistributedLockService<TDomain>(this OzzyNodeOptionsBuilder<TDomain> builder)
            where TDomain : AggregateDbContext
        {
            builder.Services.TryAddSingleton<IDistributedLockService>(sp => new EfDistributedLockService(sp.GetService<Func<TDomain>>()));
            return builder;
        }

        public static OzzyNodeOptionsBuilder<TDomain> UseEFQueues<TDomain>(this OzzyNodeOptionsBuilder<TDomain> builder)
           where TDomain : AggregateDbContext
        {
            builder.Services.TryAddSingleton<IQueueRepository>(sp => new EfQueueRepository(sp.GetService<Func<TDomain>>(), db => db.Queues));
            return builder;
        }

        public static OzzyNodeOptionsBuilder<TDomain> UseSqlServerQueues<TDomain>(this OzzyNodeOptionsBuilder<TDomain> builder)
           where TDomain : AggregateDbContext
        {
            builder.Services.TryAddSingleton<IQueueRepository>(sp => new SqlServerEfQueueRepository(sp.GetService<Func<TDomain>>(), db => db.Queues));
            return builder;
        }

        public static OzzyNodeOptionsBuilder<TDomain> UseEFFeatureFlagService<TDomain>(this OzzyNodeOptionsBuilder<TDomain> builder)
            where TDomain : AggregateDbContext
        {
            builder.Services.AddSingleton<IFeatureFlagRepository>(sp => new EfFeatureFlagRepository(sp.GetService<Func<TDomain>>(), db => db.FeatureFlags));
            builder.Services.AddTypeSpecificSingleton<FeatureFlag, ICheckpointManager>(sp => new EfCheckpointManager<TDomain>(sp.GetService<Func<TDomain>>(), "featureflags", -1));
            builder.Services.AddSingleton<FeatureFlagsEventsProcessor>();
            builder.Services.AddTypeSpecificSingleton<IDomainEventsProcessor, FeatureFlagsEventsProcessor>();
            return builder;
        }
    }
}
