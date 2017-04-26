using EventSourceProxy;
using Microsoft.EntityFrameworkCore;
using Ozzy.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ozzy.Server.EntityFramework
{
    public class EfDistributedLockService : IDistributedLockService
    {
        private readonly Func<AggregateDbContext> _dbContextFactory;
        private static IDistibutedLockEvents _logger = OzzyLogger<IDistibutedLockEvents>.LogFor<EfDistributedLockService>();

        public EfDistributedLockService(Func<AggregateDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IDistributedLock CreateLock(string name, TimeSpan expiry, Action expirationAction = null)
        {
            Guard.ArgumentNotNullOrEmptyString(name, nameof(name));
            var dlock = TryAquireLock(name, expiry, expirationAction).Result;
            return dlock ?? new EfDistributedLock(name);
        }

        public IDistributedLock CreateLock(string name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken, Action expirationAction = null)
        {
            Guard.ArgumentNotNullOrEmptyString(name, nameof(name));
            IDistributedLock result = null;
            PeriodicAction timer = null;
            timer = new PeriodicAction(async ct =>
            {
                var dlock = await TryAquireLock(name, expiry, expirationAction);
                if (dlock != null)
                {
                    result = dlock;
                    timer?.Stop();
                }
            }, Convert.ToInt32(retry.TotalMilliseconds), wait);
            cancellationToken.Register(() => timer.Stop());
            using (var scope = TraceContext.Begin())
            {
                _logger.TraceVerboseEvent($"Starting trying to acquire lock {name} for {wait} each {retry}");
                timer.Start().Wait();
                return result ?? new EfDistributedLock(name);
            }
        }

        public async Task<IDistributedLock> CreateLockAsync(string name, TimeSpan expiry, Action expirationAction = null)
        {
            Guard.ArgumentNotNullOrEmptyString(name, nameof(name));
            var dlock = await TryAquireLock(name, expiry, expirationAction);
            return dlock ?? new EfDistributedLock(name);
        }

        public async Task<IDistributedLock> CreateLockAsync(string name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken, Action expirationAction = null)
        {
            IDistributedLock result = null;
            PeriodicAction timer = null;
            timer = new PeriodicAction(async ct =>
            {
                var dlock = await TryAquireLock(name, expiry, expirationAction);
                if (dlock != null)
                {
                    result = dlock;
                    timer?.Stop();
                }
            }, Convert.ToInt32(retry.TotalMilliseconds), wait);
            cancellationToken.Register(() => timer.Stop());
            using (var scope = TraceContext.Begin())
            {
                _logger.TraceVerboseEvent($"Starting trying to acquire lock {name} for {wait} each {retry}");
                await timer.Start();
                return result ?? new EfDistributedLock(name);
            }
        }

        private async Task<EfDistributedLock> TryAquireLock(string name, TimeSpan expiry, Action expirationAction = null)
        {
            _logger.StartAcquireLock(name, expiry);
            try
            {
                using (var context = _dbContextFactory())
                {
                    var dlock = await context.DistributedLocks.SingleOrDefaultAsync(l => l.Name == name);
                    if (dlock == null)
                    {
                        dlock = new EfDistributedLockRecord(name, expiry, Guid.NewGuid());
                        context.DistributedLocks.Add(dlock);
                        dlock.Acquire(expiry);
                        var updated = await context.SaveChangesAsync();
                        if (updated > 0)
                        {
                            _logger.LockIsAcquired(name, expiry);
                            return new EfDistributedLock(_dbContextFactory, dlock, expiry, expirationAction);
                        }
                    }
                    if (!dlock.IsAcquired())
                    {
                        dlock.Acquire(expiry);
                        var updated = await context.SaveChangesAsync();
                        if (updated > 0)
                        {
                            _logger.LockIsAcquired(name, expiry);
                            return new EfDistributedLock(_dbContextFactory, dlock, expiry, expirationAction);
                        }
                    }
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.TraceVerboseEvent($"Distributed Lock {name} was not captured due to concurency exception. Exception Message was : {ex.Message}");
            }
            _logger.LockIsNotAcquired(name, expiry);
            return null;
        }
    }
}
