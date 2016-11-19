using System;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    public class DomainEventRecord
    {
        public long Sequence { get; protected set; }
        public string EventType { get; private set; }
        public string EventData { get; private set; }
        public DateTime TimeStamp { get; private set; }

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