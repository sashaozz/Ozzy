using System;
using System.Threading;

namespace Ozzy.DomainModel
{
    public interface IDistributedLock : IDisposable
    {
        bool IsAcquired { get; }
        TimeSpan Expiration { get; }
        DateTime AcquiredAt { get; }
        CancellationToken CancellationToken { get; }
        IDisposable RegisterStop(Action acion);
    }
}
