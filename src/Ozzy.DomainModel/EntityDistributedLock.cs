using System;
using System.ComponentModel.DataAnnotations;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    public class EntityDistributedLock : EntityBase<string>
    {
        public EntityDistributedLock(string name)
        {
            Guard.ArgumentNotNullOrEmptyString(name, nameof(name));
            LockId = Guid.NewGuid();
            LockDateTime = DateTime.UtcNow;
            Name = name;
        }

        [Key]
        public string Name { get; private set; }
        [ConcurrencyCheck]
        public Guid LockId { get; private set; }
        public DateTime LockDateTime { get; private set; }
        public bool IsAcquired()
        {
            return LockDateTime > DateTime.UtcNow;
        }
        public void Acquire()
        {
            LockId = Guid.NewGuid();
            LockDateTime = DateTime.UtcNow;            
        }
    }
}
