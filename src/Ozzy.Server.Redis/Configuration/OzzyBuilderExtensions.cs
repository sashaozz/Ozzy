﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ozzy.DomainModel;
using Ozzy.DomainModel.Configuration;
using Ozzy.DomainModel.Monitoring;
using Ozzy.Server.DomainDsl;
using Ozzy.Server.FeatureFlags;
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
            builder.Services.AddSingleton<MonitoringEventsProcessor>();
            builder.Services.AddSingleton<IDomainEventsProcessor, MonitoringEventsProcessor>();
            builder.Services.AddSingleton<INodesManager, NodesManager>();
            builder.Services.AddSingleton<IMonitoringRepository, RedisMonitoringRepository>();
            builder.AddBackgroundProcess<NodesMonitoringProcess>();
            return builder;
        }
    }
}
