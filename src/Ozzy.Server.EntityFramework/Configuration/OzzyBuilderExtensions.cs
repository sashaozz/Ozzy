using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
using Ozzy.Server.EntityFramework;
using System;

namespace Ozzy.Server.Configuration
{
    public static class OzzyBuilderExtensions
    {
        public static IOzzyBuilder UseEFDistributedLockService<TDomain>(this IOzzyBuilder builder)
            where TDomain : AggregateDbContext
        {
            builder.Services.AddSingleton<IDistributedLockService, EfDistributedLockService>(sp => new EfDistributedLockService(sp.GetService<Func<TDomain>>()));
            return builder;
        }

        public static IOzzyBuilder UseEFFeatureFlagService<TDomain>(this IOzzyBuilder builder)
            where TDomain : AggregateDbContext
        {
            builder.Services.AddSingleton<IFeatureFlagRepository>(sp => new EfFeatureFlagRepository(sp.GetService<Func<TDomain>>(), db => db.FeatureFlags));
            builder.Services.AddSingleton(sp => new TypedRegistration<FeatureFlag, ICheckpointManager>(new EfCheckpointManager<TDomain>(sp.GetService<Func<TDomain>>(), "featureflags", -1)));
            builder.Services.AddSingleton<FeatureFlagsEventsProcessor>();
            builder.Services.AddTypeSpecificSingleton<IDomainEventsProcessor, FeatureFlagsEventsProcessor>();
            return builder;
        }

        public static IOzzyBuilder UseEFBackgroundTaskService<TDomain>(this IOzzyBuilder builder)
           where TDomain : AggregateDbContext
        {
            builder.Services.AddSingleton<IQueueRepository>(sp => new SqlServerEfQueueRepository(sp.GetService<Func<TDomain>>(), db => db.Queues));
            builder.AddBackgroundProcess<TaskQueueProcess>();
            return builder;
        }
    }
}
