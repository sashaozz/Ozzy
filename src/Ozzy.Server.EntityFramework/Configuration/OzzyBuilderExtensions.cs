using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
using Ozzy.Server.BackgroundTasks;
using Ozzy.Server.DomainDsl;
using Ozzy.Server.EntityFramework;
using Ozzy.Server.FeatureFlags;
using System;

namespace Ozzy.Server.Configuration
{
    public static class OzzyBuilderExtensions
    {
        public static IOzzyBuilder UseEFDistributedLockService<TDomain>(this IOzzyBuilder builder)
            where TDomain : AggregateDbContext
        {
            builder.Services.AddSingleton<IDistributedLockService, EntityDistributedLockService>(sp => new EntityDistributedLockService(sp.GetService<Func<TDomain>>()));
            return builder;
        }

        public static IOzzyBuilder UseEFFeatureFlagService<TDomain>(this IOzzyBuilder builder)
            where TDomain : AggregateDbContext
        {
            builder.Services.AddSingleton<IFeatureFlagRepository>(sp => new FeatureFlagRepository(sp.GetService<Func<TDomain>>(), db => db.FeatureFlags));
            builder.Services.AddSingleton(sp => new TypedRegistration<FeatureFlag, ICheckpointManager>(new DbCheckpointManager(sp.GetService<Func<TDomain>>(), "featureflags", -1)));            
            builder.Services.AddSingleton<FeatureFlagsEventsProcessor>();
            builder.Services.AddSingleton<IDomainEventsProcessor, FeatureFlagsEventsProcessor>();            
            builder.Services.AddSingleton<OzzyNodeEventLoop<TDomain>>();
            return builder;
        }

        public static IOzzyBuilder UseEFBackgroundTaskService<TDomain>(this IOzzyBuilder builder)
           where TDomain : AggregateDbContext
        {
            builder.Services.AddSingleton<IBackgroundTaskRepository>(sp => new BackgroundTaskRepository(sp.GetService<Func<TDomain>>(), db => db.BackgroundTasks));
            return builder;
        }
    }
}
