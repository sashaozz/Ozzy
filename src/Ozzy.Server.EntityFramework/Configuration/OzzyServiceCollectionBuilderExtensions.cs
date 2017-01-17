using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
using Ozzy.DomainModel.Configuration;
using Ozzy.Server.EntityFramework;
using Ozzy.Server.FeatureFlags;

namespace Ozzy.Server.Configuration
{
    public static class OzzyServiceCollectionBuilderExtensions
    {        
        public static IOzzyBuilder UseEF(this IOzzyBuilder builder, IConfiguration conf)
        {
            builder.Services.Configure<EntityFrameworkConnectionOptions>(conf.GetSection("EntityFramework"));
            //builder.Services.AddSingleton<RedisClient>(sc => new RedisClient(sc.GetService<IOptions<RedisConnectionOptions>>()));
            return builder;
        }

        public static IOzzyBuilder UseEFDistributedLockService(this IOzzyBuilder builder)
        {
            builder.Services.AddSingleton<IDistributedLockService, EntityDistributedLockService>();
            return builder;     
        }

        public static IOzzyBuilder UseEFFeatureFlagService(this IOzzyBuilder builder)
        {
            builder.Services.AddSingleton<IFeatureFlagRepository>(sp => new FeatureFlagRepository(sp.GetService<TransientAggregateDbContext>(), db => db.FeatureFlags));
            return builder;
        }
    }
}
