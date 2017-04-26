using System;
using System.IO;
using ProtoBuf;
using StackExchange.Redis;
using Ozzy.DomainModel;
using Ozzy.Core;
using System.Threading.Tasks;

namespace Ozzy.Server.Redis
{
    public class RedisFastEventReciever : BackgroundTask,  IFastEventReciever
    {
        private string _channel;
        private ISubscriber _subscriber;
        private RedisClient _redis;
        private Action<IDomainEventRecord> _consumerAction;
        private DomainEventsLoop _loop;

        public RedisFastEventReciever(RedisClient redis, DomainEventsLoop loop, string channelName)            
        {
            Guard.ArgumentNotNull(redis, nameof(redis));
            Guard.ArgumentNotNull(loop, nameof(loop));
            Guard.ArgumentNotNullOrEmptyString(channelName, nameof(channelName));

            _redis = redis;
            _loop = loop;
            _channel = channelName;//$"{options.OptionsType.FullName}-fast-channel";
        }

        public void Recieve(IDomainEventRecord message)
        {
            _loop.AddEventForProcessing(message);
        }
       
        public void StartRecieving()
        {
            Start();
        }

        public void StopRecieving()
        {
            Stop();
        }

        protected override Task StartInternal()
        {
            _subscriber = _redis.Redis.GetSubscriber();
            _subscriber.SubscribeAsync(_channel, (ch, message) =>
            {
                IDomainEventRecord data;
                using (var stream = new MemoryStream(message))
                {
                    data = Serializer.Deserialize<IDomainEventRecord>(stream);
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
