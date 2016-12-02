using System;
using System.IO;
using ProtoBuf;
using StackExchange.Redis;
using Ozzy.DomainModel;

namespace Ozzy.Server.Redis
{
    public class RedisEventsReciever : IDisposable
    {
        private readonly string _channel;
        
        private readonly ISubscriber _subscriber;

        public RedisEventsReciever(RedisClient redis, Action<DomainEventRecord> consumerAction, string channel = "fast-events-bus")
        {
            
            if (redis == null) throw new ArgumentNullException(nameof(redis));
            if (consumerAction == null) throw new ArgumentNullException(nameof(consumerAction));

            _channel = channel;
            _subscriber = redis.Redis.GetSubscriber();
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
                consumerAction(data);

            });
        }

        public void Dispose()
        {
            _subscriber.Unsubscribe(_channel);            
        }
    }
}
