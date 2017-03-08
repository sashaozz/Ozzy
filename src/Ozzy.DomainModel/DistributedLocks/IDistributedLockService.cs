using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.DomainModel
{
    public interface IDistributedLockService
    {
        IDistributedLock CreateLock(string name, TimeSpan expiry, Action expirationAction = null);
        IDistributedLock CreateLock(string name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken, Action expirationAction = null);

        Task<IDistributedLock> CreateLockAsync(string name, TimeSpan expiry, Action expirationAction = null);
        Task<IDistributedLock> CreateLockAsync(string name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken, Action expirationAction = null);
    }
}