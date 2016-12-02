using System;
using System.Threading;

namespace Ozzy.DomainModel
{
    public interface IDistributedLock : IDisposable
    {
        bool IsAcquired { get; }
        DateTime ExpirationTime { get; }
        TimeSpan Expiry { get; }
    }
}
