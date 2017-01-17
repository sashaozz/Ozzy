using System;
using System.Linq;
using Ozzy.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ozzy.DomainModel;
using System.Threading;

namespace Ozzy.Server.EntityFramework
{
    public class EntityDistributedLockService : IDistributedLockService
    {
        private readonly TransientAggregateDbContext _dbContext;

        public EntityDistributedLockService(TransientAggregateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IDistributedLock CreateLock(string name, TimeSpan expiry, Action expirationAction = null)
        {
            Guard.ArgumentNotNullOrEmptyString(name, nameof(name));
            Guard.ArgumentNotNull(expiry, nameof(expiry));
            try
            {
                var guid = Guid.NewGuid();
                var context = _dbContext.Clone();
                var dlock = context.DistributedLocks.SingleOrDefault(l => l.Name == name)
                    ?? new EntityDistributedLockRecord(name, expiry, guid);
                if (dlock.LockId == guid || !dlock.IsAcquired())
                {
                    dlock.Acquire(expiry);
                    context.SaveChanges();
                    return new EntityDistributedLock(context, dlock, expiry, expirationAction);
                }
                else
                {
                    return new EntityDistributedLock();
                }
            }
            catch (Exception e)
            {
                //todo : log exception
                return new EntityDistributedLock(e);
            }
        }

        public IDistributedLock CreateLock(string name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, Action expirationAction = null)
        {
            IDistributedLock result = new EntityDistributedLock();
            PeriodicAction timer = null;
            try
            {
                var guid = Guid.NewGuid();
                var context = _dbContext.Clone();
                var dlock = context.DistributedLocks.SingleOrDefault(l => l.Name == name);
                if (dlock == null)
                {
                    dlock = new EntityDistributedLockRecord(name, expiry, guid);
                    context.DistributedLocks.Add(dlock);
                }
                    

                //todo : add period 
                timer = new PeriodicAction(cts =>
                {
                    try
                    {
                        context.Entry(dlock).Reload();
                        if (!dlock.IsAcquired())
                        {
                            dlock.Acquire(expiry);
                            context.SaveChanges();
                            result = new EntityDistributedLock(context, dlock, expiry, expirationAction);
                            cts.Cancel();
                            return Task.CompletedTask;
                        }
                        else
                        {
                            return Task.CompletedTask;
                        }
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        //todo : log exception
                        return Task.CompletedTask;
                    }
                    catch (Exception e)
                    {
                        //todo : log exception
                        result = new EntityDistributedLock();
                        cts.Cancel();
                        return Task.CompletedTask;
                    }
                });

                if (dlock.LockId == guid || !dlock.IsAcquired())
                {
                    dlock.Acquire(expiry);
                    context.SaveChanges();
                    return new EntityDistributedLock(context, dlock, expiry, expirationAction);
                }
                else
                {
                    timer?.Start().Wait();
                    return result;
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                //todo : log exception
                timer?.Start().Wait();
                return result;
            }
            catch (Exception e)
            {
                //todo : log exception
            }
            return result;
        }

        public Task<IDistributedLock> CreateLockAsync(string name, TimeSpan expiry, Action expirationAction = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IDistributedLock> CreateLockAsync(string name, TimeSpan expiry, TimeSpan wait, TimeSpan retry, CancellationToken cancellationToken, Action expirationAction = null)
        {
            IDistributedLock result = new EntityDistributedLock();
            PeriodicAction timer = null;
            try
            {
                var guid = Guid.NewGuid();
                var context = _dbContext.Clone();
                var dlock = await context.DistributedLocks.SingleOrDefaultAsync(l => l.Name == name);
                if (dlock == null)
                {
                    dlock = new EntityDistributedLockRecord(name, expiry, guid);
                    await context.DistributedLocks.AddAsync(dlock);
                }


                //todo : add period 
                timer = new PeriodicAction(cts =>
                {
                    try
                    {
                        context.Entry(dlock).Reload();
                        if (!dlock.IsAcquired())
                        {
                            dlock.Acquire(expiry);
                            context.SaveChanges();
                            result = new EntityDistributedLock(context, dlock, expiry, expirationAction);
                            cts.Cancel();
                            return Task.CompletedTask;
                        }
                        else
                        {
                            return Task.CompletedTask;
                        }
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        //todo : log exception
                        return Task.CompletedTask;
                    }
                    catch (Exception e)
                    {
                        //todo : log exception
                        result = new EntityDistributedLock();
                        cts.Cancel();
                        return Task.CompletedTask;
                    }
                });

                if (dlock.LockId == guid || !dlock.IsAcquired())
                {
                    dlock.Acquire(expiry);
                    await context.SaveChangesAsync(cancellationToken);
                    return new EntityDistributedLock(context, dlock, expiry, expirationAction);
                }
                else
                {
                    await timer?.Start();
                    return result;
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                //todo : log exception
                await timer?.Start();
                return result;
            }
            catch (Exception e)
            {
                //todo : log exception
            }
            return result;
        }
    }
}
