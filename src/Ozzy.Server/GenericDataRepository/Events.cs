using System;

namespace Ozzy.DomainModel
{
    public class DataRecordCreatedEvent<TItem> : IDomainEvent
    {        
        //public Type RecordType { get; set; }
        public TItem RecordValue { get; set; }
    }
    public class DataRecordUpdatedEvent<TItem> : IDomainEvent
    {
        //public Type RecordType { get; set; }
        public TItem RecordValue { get; set; }
    }
    public class DataRecordDeletedEvent<TItem> : IDomainEvent
    {
        //public Type RecordType { get; set; }
        public TItem RecordValue { get; set; }
    }
}
