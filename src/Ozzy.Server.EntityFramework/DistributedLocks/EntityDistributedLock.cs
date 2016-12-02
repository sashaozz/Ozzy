using Ozzy.Core;
using Ozzy.DomainModel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.Server.EntityFramework
{
    public class EntityDistributedLock : IDistributedLock
    {
        private EntityDistributedLockRecord _record;
        private AggregateDbContext _context;
        private readonly Action _action;
        private PeriodicAction _timer;
        private bool _disposed;

        public bool IsAcquired { get; private set; }
        public TimeSpan Expiry { get; private set; }
        public DateTime ExpirationTime { get; private set; }

        public EntityDistributedLock(AggregateDbContext context, EntityDistributedLockRecord record, TimeSpan expiration, Action expirationAction)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            Guard.ArgumentNotNull(record, nameof(record));
            Guard.ArgumentNotNull(expiration, nameof(expiration));

            _action = expirationAction;
            _context = context;
            _record = record;
            IsAcquired = true;
            Expiry = expiration;
            ExpirationTime = record.LockDateTime;           

            _timer = new PeriodicAction(cts => ExtendLockAsync(cts), expiration.Milliseconds);
        }

        public EntityDistributedLock()
        {
            IsAcquired = false;
        }

        public EntityDistributedLock(Exception e)
        {
            IsAcquired = false;
        }

        private async Task ExtendLockAsync(CancellationTokenSource cts)
        {
            try
            {
                _record.Acquire(Expiry);
                await _context.SaveChangesAsync(cts.Token);
            }
            catch (Exception e)
            {
                //todo : log exception
                Dispose();
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            IsAcquired = false;
            ReleaseLock();
            _timer.Dispose();
            _action?.Invoke();
            _disposed = true;
        }

        private void ReleaseLock()
        {
            //todo : implement release lock
        }
    }
}
