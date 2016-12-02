using System;
using System.ComponentModel.DataAnnotations;
using Ozzy.Core;
using Ozzy.DomainModel;

namespace Ozzy.Server.EntityFramework
{
    public class EntityDistributedLockRecord : EntityBase<string>
    {
        private TimeSpan _expiration;

        public EntityDistributedLockRecord(string name, TimeSpan expiration,  Guid lockId)
        {
            Guard.ArgumentNotNullOrEmptyString(name, nameof(name));
            _expiration = expiration;
            LockId = lockId;
            LockDateTime = DateTime.UtcNow.Add(_expiration);
            Name = name;
        }
        protected EntityDistributedLockRecord() { }

        [Key]
        public string Name { get; private set; }
        [ConcurrencyCheck]
        public Guid LockId { get; private set; }
        public DateTime LockDateTime { get; private set; }
        public bool IsAcquired()
        {
            return LockDateTime > DateTime.UtcNow;
        }
        public void Acquire(TimeSpan expiry)
        {
            LockId = Guid.NewGuid();
            LockDateTime = DateTime.UtcNow.Add(expiry);
        }
    }
}
