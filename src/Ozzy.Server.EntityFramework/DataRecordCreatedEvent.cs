using Ozzy.DomainModel;
using System;

namespace Ozzy.Server.EntityFramework
{
    public class DataRecordCreatedEvent : IDomainEvent
    {
        public Type RecordType { get; set; }
        public object RecordValue { get; set; }
    }    
}
