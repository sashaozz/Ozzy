using Microsoft.Extensions.Options;
using Ozzy.DomainModel.Configuration;
using StackExchange.Redis;
using System;

namespace Ozzy.Server.Redis
{
    public class RedisClient
    {
        public IConnectionMultiplexer Redis { get; private set; }
        public RedisClient(Func<IConnectionMultiplexer> redis)
        {
            Redis = redis();
        }

        public RedisClient(IOptions<RedisConnectionOptions> options)
        {
            Redis = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
        }
    }
}
