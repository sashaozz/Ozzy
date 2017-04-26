using System;
using System.Collections.Generic;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Пустая запись о доменном событии. Служит для заполнения пробелов в последовательности доменных событий, 
    /// которые могут появляться из-за особенностей реализации identity в SqlServer
    /// </summary>
    public class EmptyEventRecord : IDomainEventRecord
    {
        public EmptyEventRecord(long sequence)
        {
            Sequence = sequence;
        }
        public long Sequence { get; private set; }
        public Dictionary<string, object> MetaData { get; private set; } = new Dictionary<string, object>();
        public DateTime TimeStamp => DateTime.MinValue;
        public object GetDomainEvent() => null;
        public T GetDomainEvent<T>() => default(T);
        public Type GetDomainEventType() => null;
    }
}