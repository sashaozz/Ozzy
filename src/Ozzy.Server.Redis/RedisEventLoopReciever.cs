using System;
using System.IO;
using ProtoBuf;
using StackExchange.Redis;
using Ozzy.DomainModel;
using Ozzy.Core;
using System.Threading.Tasks;

namespace Ozzy.Server.Redis
{
    public class RedisEventLoopReciever<TLoop, TDomain> : EventLoopReciever<TLoop, TDomain>
        where TLoop : DomainEventLoop<TDomain>
        where TDomain : IOzzyDomainModel
    {
        private string _channel;
        private ISubscriber _subscriber;
        private RedisClient _redis;
        private Action<DomainEventRecord> _consumerAction;

        public RedisEventLoopReciever(RedisClient redis, IExtensibleOptions<TDomain> options, TLoop loop)
            : base(options, loop)
        {
            Guard.ArgumentNotNull(redis, nameof(redis));
            Guard.ArgumentNotNull(options, nameof(options));
            Guard.ArgumentNotNull(loop, nameof(loop));

            _redis = redis;
            _channel = $"{options.OptionsType.FullName}-fast-channel";
        }     

        protected override Task StartInternal()
        {
            _subscriber = _redis.Redis.GetSubscriber();
            _subscriber.SubscribeAsync(_channel, (ch, message) =>
            {
                DomainEventRecord data;
                using (var stream = new MemoryStream(message))
                {
                    data = Serializer.Deserialize<DomainEventRecord>(stream);
                }
                if (data == null)
                {
                    //todo: add logging
                    return;
                }
                Recieve(data);
            });
            return base.StartInternal();
        }
        protected override void StopInternal()
        {
            _subscriber.UnsubscribeAll();
            base.StopInternal();
        }
    }    
}
