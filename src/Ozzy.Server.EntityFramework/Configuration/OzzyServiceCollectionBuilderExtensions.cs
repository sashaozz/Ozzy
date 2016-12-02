using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
using Ozzy.DomainModel.Configuration;
using Ozzy.Server.EntityFramework;

namespace Ozzy.Server.Configuration
{
    public static class OzzyServiceCollectionBuilderExtensions
    {        
        public static IOzzyServiceCollectionBuilder UseEF(this IOzzyServiceCollectionBuilder builder, IConfiguration conf)
        {
            builder.Services.Configure<EntityFrameworkConnectionOptions>(conf.GetSection("EntityFramework"));
            //builder.Services.AddSingleton<RedisClient>(sc => new RedisClient(sc.GetService<IOptions<RedisConnectionOptions>>()));
            return builder;
        }

        public static IOzzyServiceCollectionBuilder UseEFDistributedLockService(this IOzzyServiceCollectionBuilder builder)
        {
            builder.Services.AddSingleton<IDistributedLockService, EntityDistributedLockService>();
            return builder;     
        }
    }
}
