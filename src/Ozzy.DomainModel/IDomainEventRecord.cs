using System;

namespace Ozzy.DomainModel
{
    public interface IDomainEventRecord
    {
        long Sequence { get; }
        DateTime TimeStamp { get; }
        object GetDomainEvent();
        T GetDomainEvent<T>();
        Type GetDomainEventType();
    }    
}