using System;
using Ozzy.Core;
using System.Collections.Generic;

namespace Ozzy.DomainModel
{
    public interface IDomainEventRecord
    {
        long Sequence { get; }
        DateTime TimeStamp { get; }
        Dictionary<string, object> MetaData { get; }
        object GetDomainEvent();
        T GetDomainEvent<T>();
        Type GetDomainEventType();
    }
    public class DomainEventRecord : IDomainEventRecord
    {
        public long Sequence { get; set; }
        public string EventType { get; set; }
        public string EventData { get; set; }
        public DateTime TimeStamp { get; set; }

        public Dictionary<string, object> MetaData { get; set; } = new Dictionary<string, object>();

        public DomainEventRecord(object @event)
        {
            Guard.ArgumentNotNull(@event, nameof(@event));
            EventType = @event.GetType().AssemblyQualifiedName;
            TimeStamp = DateTime.UtcNow;
            EventData = EventSerializer.Serialize(@event);
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
            return EventSerializer.Deserialize(EventData, t);
        }
        public T GetDomainEvent<T>()
        {
            return EventSerializer.Deserialize<T>(EventData);
        }

        public Type GetDomainEventType()
        {
            return Type.GetType(EventType);
        }
    }

}