using Ozzy.DomainModel;
using Ozzy.Core;
using System;

namespace Ozzy.Server.Redis
{
    public class RedisFastEventRecieverFactory : IFastEventRecieverFactory        
    {
        private RedisClient _redis;
        private string _channelName;

        public RedisFastEventRecieverFactory(RedisClient redis, string channelName)
        {
            Guard.ArgumentNotNull(redis, nameof(redis));
            Guard.ArgumentNotNullOrEmptyString(channelName, nameof(channelName));
            _redis = redis;
            _channelName = channelName;
        }

        public IFastEventReciever CreateReciever(DomainEventsManager loop)
        {
            return new RedisFastEventReciever(_redis, loop, _channelName);
        }
    }
}
