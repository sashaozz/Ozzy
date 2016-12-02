using System;
using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;
using StackExchange.Redis;
using Ozzy.DomainModel;

namespace Ozzy.Server.Redis
{
    public class RedisPubSubEventPublisher : IFastEventPublisher
    {
        static RedisPubSubEventPublisher()
        {
            RuntimeTypeModel.Default.Add(typeof (DomainEventRecord), false)
                .Add("Sequence", "AggregateId", "EventType", "EventData", "TimeStamp");
        }

        private readonly string _channel;
        private readonly IDatabase _db;

        public RedisPubSubEventPublisher(RedisClient redis, string channel = "fast-events-bus")
        {
            if (redis == null) throw new ArgumentNullException(nameof(redis));
            if (channel == null) throw new ArgumentNullException(nameof(channel));
            _channel = channel;
            _db = redis.Redis.GetDatabase();
        }

        public void Publish(DomainEventRecord message)
        {
            
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream,message);
                _db.Publish(_channel, stream.ToArray(), CommandFlags.FireAndForget);
            }
        }
    }
}
