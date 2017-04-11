using Microsoft.EntityFrameworkCore;
using Ozzy.Core;
using Ozzy.Core.Extensions;
using Ozzy.DomainModel;
using Ozzy.Server.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Ozzy.Server.EntityFramework
{
    public class EntityDistributedLock : IDistributedLock
    {
        private readonly Action _action;
        private PeriodicAction _timer;
        private bool _disposed;
        private Func<AggregateDbContext> _contextFactory;
        private static IDistibutedLockEvents _logger = OzzyLogger<IDistibutedLockEvents>.LogFor<EntityDistributedLockService>();
        public bool IsAcquired { get; protected set; }
        public TimeSpan Expiry { get; protected set; }
        public string LockName { get; protected set; }
        public Guid LockId { get; protected set; }
        public DateTime ExpirationTime { get; protected set; }

        public EntityDistributedLock(Func<AggregateDbContext> contextFactory, EntityDistributedLockRecord record, TimeSpan expiration, Action expirationAction = null)
        {
            Guard.ArgumentNotNull(contextFactory, nameof(contextFactory));
            Guard.ArgumentNotNull(record, nameof(record));
            Guard.ArgumentNotNull(expiration, nameof(expiration));

            _action = expirationAction;
            _contextFactory = contextFactory;
            Expiry = expiration;
            ExpirationTime = record.LockDateTime;
            IsAcquired = record.IsAcquired();
            LockName = record.Name;
            LockId = record.LockId;
            var extendTime = (expiration.TotalMilliseconds * 0.9).Do(Convert.ToInt32);
            _timer = new PeriodicAction(ct => ExtendLockAsync(ct), extendTime, null, true);
            _timer.Start();
        }

        public EntityDistributedLock(string name)
        {
            IsAcquired = false;
            LockName = name;
        }

        public EntityDistributedLock(Exception e)
        {
            IsAcquired = false;
        }

        private async Task ExtendLockAsync(CancellationToken ct)
        {
            _logger.TraceVerboseEvent($"Starting extending lock {LockName} with id = {LockId} for {Expiry}");
            try
            {
                using (var context = _contextFactory())
                {
                    var dlock = await context.DistributedLocks.SingleOrDefaultAsync(l => l.LockId == LockId);
                    if (dlock != null)
                    {
                        dlock.Acquire(Expiry);
                        var updated = await context.SaveChangesAsync(ct);
                        if (updated > 0)
                        {
                            LockId = dlock.LockId;
                            ExpirationTime = dlock.LockDateTime;
                            IsAcquired = dlock.IsAcquired();
                            _logger.LockIsExtended(LockName, Expiry);
                            return;
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                //todo: sadsa
            }
            catch (Exception e)
            {
                //todo:handle exception serialization
                //_logger.Exception(e, $"Exception when trying to extend distributed lock {LockName}");
                //_logger.TraceVerboseEvent($"Exception when trying to extend distributed lock {LockName}");
            }
            Dispose();
            _action?.Invoke();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _timer?.Dispose();
            if (IsAcquired)
            {
                TryReleaseLock();                
                IsAcquired = false;
            }                                    
            _disposed = true;
        }

        private void TryReleaseLock()
        {
            _logger.TraceVerboseEvent($"LockReleased");
            //_logger.LockReleased(LockName, ExpirationTime);            
            try
            {
                using (var context = _contextFactory())
                {
                    var dlock = context.DistributedLocks.SingleOrDefault(l => l.LockId == LockId);
                    if (dlock != null && dlock.IsAcquired())
                    {
                        dlock.Release();
                        context.SaveChanges();
                    }
                }
            }
            catch
            {
                //todo: log
            }
        }
    }
}