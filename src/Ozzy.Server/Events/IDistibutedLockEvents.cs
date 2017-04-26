using EventSourceProxy;
using Ozzy.Core;
using System;
using System.Diagnostics.Tracing;

namespace Ozzy.Server
{
    [EventSourceImplementation(Name = "Ozzy-DistributedLockEvents")]
    public interface IDistibutedLockEvents : ICommonEvents
    {
        [Event(1001, Level = EventLevel.Verbose, Message = "Starting acquiring lock {0} for {1}")]        
        void StartAcquireLock(string LockName, TimeSpan LockExpiry);
        [Event(1002, Level = EventLevel.Verbose, Message = "Lock {0} is acquired for {1}")]
        void LockIsAcquired(string LockName, TimeSpan LockExpiry);
        [Event(1003, Level = EventLevel.Verbose, Message = "Lock {0} is NOT acquired for {1}")]
        void LockIsNotAcquired(string LockName, TimeSpan LockExpiry);
        [Event(1004, Level = EventLevel.Verbose, Message = "Lock {0} was axtented for another {1}")]
        void LockIsExtended(string LockName, TimeSpan LockExpiry);
        [Event(1005, Level = EventLevel.Verbose, Message = "Lock {0} was released. Expiration time is {1}")]
        void LockReleased(string LockName, DateTime ExpirationTime);

    }
}
