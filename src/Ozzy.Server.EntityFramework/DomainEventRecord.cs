using System;
using System.Collections.Generic;
using Ozzy.DomainModel;

namespace Ozzy.Server.EntityFramework
{    
    public class DomainEventRecord : IDomainEventRecord
    {
        public static ISerializer Serializer = new ContractlessMessagePackSerializer();
        public long Sequence { get; set; }
        public string EventType { get; set; }
        public byte[] EventData { get; set; }
        public DateTime TimeStamp { get; set; }

        public Dictionary<string, object> MetaData { get; set; } = new Dictionary<string, object>();

        public DomainEventRecord(object @event)
        {
            Guard.ArgumentNotNull(@event, nameof(@event));
            EventType = @event.GetType().AssemblyQualifiedName;
            TimeStamp = DateTime.UtcNow;
            EventData = Serializer.BinarySerialize(@event, @event.GetType());
        }
        public DomainEventRecord(object @event, long sequence) : this(@event)
        {
            Sequence = sequence;
        }

        protected DomainEventRecord()
        {
        }

        public object GetDomainEvent()
        {
            var t = Type.GetType(EventType);
            return Serializer.BinaryDeSerialize(EventData, t);
        }
        public T GetDomainEvent<T>()
        {
            return Serializer.BinaryDeSerialize<T>(EventData);
        }

        public Type GetDomainEventType()
        {
            return Type.GetType(EventType);
        }
    }
}
