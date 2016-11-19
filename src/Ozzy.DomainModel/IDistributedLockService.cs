using System;

namespace Ozzy.DomainModel
{
    public interface IDistributedLockService
    {
        IDistributedLock CreateLock(string name, TimeSpan timeout);
    }
}