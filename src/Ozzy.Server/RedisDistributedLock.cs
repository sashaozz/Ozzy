using Ozzy.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Ozzy.Server
{
    public class RedisDistributedLock : IDistributedLock
    {
        public RedisDistributedLock(bool isAcquired, TimeSpan expiration, DateTime acquiredAt)
        {
            IsAcquired = isAcquired;
            Expiration = expiration;
            AcquiredAt = acquiredAt;
        }

        public DateTime AcquiredAt { get; }

        public CancellationToken CancellationToken { get; set; }
  

        public TimeSpan Expiration { get; }

        public bool IsAcquired { get; }

        public void Dispose()
        {
        }

        public IDisposable RegisterStop(Action acion)
        {
            return null;
        }
    }
}
