using System;

namespace Ozzy.Server
{
    public interface IDistributedLock : IDisposable
    {
        bool IsAcquired { get; }
        DateTime ExpirationTime { get; }
        TimeSpan Expiry { get; }
    }
}
