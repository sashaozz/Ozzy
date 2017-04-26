using System;
using System.IO;
using ProtoBuf;
using StackExchange.Redis;
using Ozzy.DomainModel;
using Ozzy.Core;

namespace Ozzy.Server.Redis
{
    public class RedisEventsReciever : IFastEventReciever
    {
        private string _channel;
        private ISubscriber _subscriber;
        private RedisClient _redis;
        private Action<IDomainEventRecord> _consumerAction;

        public RedisEventsReciever(RedisClient redis, Action<IDomainEventRecord> consumerAction = null, string channel = "DomainEventsManager-fast-channel")
        {
            Guard.ArgumentNotNull(redis, nameof(redis));
            Guard.ArgumentNotNull(consumerAction, nameof(consumerAction));
            Guard.ArgumentNotNullOrEmptyString(channel, nameof(channel));

            _redis = redis;
            _channel = channel;
            _consumerAction = consumerAction;
        }

        public void UseAction(Action<IDomainEventRecord> consumerAction)
        {
            Guard.ArgumentNotNull(consumerAction, nameof(consumerAction));
            _consumerAction = consumerAction;
        }

        public virtual void Recieve(IDomainEventRecord message)
        {
            _consumerAction?.Invoke(message);
        }

        public void Dispose()
        {
            StopRecieving();
        }

        public void StartRecieving()
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
        }

        public void StopRecieving()
        {
            _subscriber.Unsubscribe(_channel);
        }        
    }
}
