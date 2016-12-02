using System;

namespace Ozzy.DomainModel
{
    public interface IDistributedLockService
    {
        IDistributedLock CreateLock(string name, TimeSpan expiry, Action expirationAction = null);
        IDistributedLock CreateLock(string name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, Action expirationAction = null);
    }
}