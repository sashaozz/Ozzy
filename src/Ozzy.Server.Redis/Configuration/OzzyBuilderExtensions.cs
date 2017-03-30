using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ozzy.DomainModel.Configuration;
using Ozzy.Server.Monitoring;
using Ozzy.Server.Redis;
using Ozzy.Server.Redis.Monitoring;
using StackExchange.Redis;
using System;

namespace Ozzy.Server.Configuration
{
    public static class OzzyBuilderExtensions
    {        

        public static IOzzyBuilder UseRedis(this IOzzyBuilder builder, Func<IConnectionMultiplexer> redisFactory)
        {
            builder.Services.AddSingleton<RedisClient>(sc => new RedisClient(redisFactory));
            return builder;
        }

        public static IOzzyBuilder UseRedis(this IOzzyBuilder builder, IConfiguration conf)
        {
            builder.Services.Configure<RedisConnectionOptions>(conf.GetSection("RedisConnection"));
            builder.Services.AddSingleton<RedisClient>(sc => new RedisClient(sc.GetService<IOptions<RedisConnectionOptions>>()));
            return builder;
        }

        public static IOzzyBuilder UseRedisMonitoring(this IOzzyBuilder builder)
        {
            builder.Services.AddTransient<IMonitoringManager, RedisMoniitoringManager>();
            return builder;
        }
    }
}
