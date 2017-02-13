using Ozzy.DomainModel;
using Ozzy.Core;

namespace Ozzy.Server.Redis
{
    public class RedisEventLoopRecieverFactory<TDomain> : IFastEventRecieverFactory<TDomain>
        where TDomain : IOzzyDomainModel
    {
        private RedisClient _redis;
        private IExtensibleOptions<TDomain> _options;

        public RedisEventLoopRecieverFactory(RedisClient redis, IExtensibleOptions<TDomain> options)
        {
            Guard.ArgumentNotNull(redis, nameof(redis));
            _redis = redis;
            _options = options;
        }

        public IFastEventReciever<TLoop> CreateReciever<TLoop>()
            where TLoop : DomainEventLoop<TDomain>
        {
            var loop = _options.GetService<TLoop>();
            return new RedisEventLoopReciever<TLoop, TDomain>(_redis, _options, loop);
        }       
    }
}
