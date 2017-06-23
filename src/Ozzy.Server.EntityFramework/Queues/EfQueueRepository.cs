using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Ozzy.Server.Queues;

namespace Ozzy.Server.EntityFramework
{
    public class EfQueueRepository : IQueueRepository
    {
        private Func<AggregateDbContext> _dbFactory;
        private Func<AggregateDbContext, DbSet<QueueRecord>> _dbSetProvider;
        private Func<AggregateDbContext, DbSet<DeadLetter>> _deadLetterDbSetProvider;

        public EfQueueRepository(Func<AggregateDbContext> dbFactory
            , Func<AggregateDbContext, DbSet<QueueRecord>> dbSetProvider
            , Func<AggregateDbContext, DbSet<DeadLetter>> deadLetterDbSetProvider)
        {
            Guard.ArgumentNotNull(dbFactory, nameof(dbFactory));
            Guard.ArgumentNotNull(dbSetProvider, nameof(dbSetProvider));
            Guard.ArgumentNotNull(dbSetProvider, nameof(deadLetterDbSetProvider));
            _dbFactory = dbFactory;
            _dbSetProvider = dbSetProvider;
            _deadLetterDbSetProvider = deadLetterDbSetProvider;

        }

        public string Put(string queueName, byte[] item, int maxRetries = 5)
        {
            var record = new QueueRecord()
            {
                Payload = item,
                QueueName = queueName,
                RetryCount = 0,
                MaxRetries = maxRetries
            };

            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                dbSet.Add(record);
                db.SaveChanges();
            }
            return record.Id;
        }

        public virtual QueueItem Fetch(string queueName, long acknowledgeTimeOut = 60)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);

                var item = dbSet
                    .Where(s => s.QueueName == queueName)
                    .Where(s => s.Status == QueueStatus.Queued)
                    .OrderBy(i => i.CreatedAt)
                    .FirstOrDefault();
                if (item == null) return null;

                var settings = item as IQueueFaultSettings;

                item.Status = QueueStatus.Fetched;
                item.TimeoutAt = DateTime.UtcNow.AddSeconds(acknowledgeTimeOut);

                db.SaveChanges();
                return new QueueItem(item.Id, item.Payload)
                {
                    CreatedAt = item.CreatedAt,
                    RetryCount = item.RetryCount,
                    TimeoutAt = item.TimeoutAt,
                    QueueName = item.QueueName,
                    MaxRetries = item.MaxRetries
                };
            }
        }

        public void Acknowledge(string id, string queueName)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                var dbItem = dbSet.Single(i => i.Id.Equals(id));
                dbSet.Remove(dbItem);
                db.SaveChanges();
            }
        }

        public List<QueueItem> GetTimeoutedItems()
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);

                var items = dbSet
                    .Where(s => s.Status == QueueStatus.Fetched && s.TimeoutAt < DateTime.UtcNow)
                    .ToList();

                return items.Select(i => new QueueItem(i.Id, i.Payload)
                {
                    CreatedAt = i.CreatedAt,
                    RetryCount = i.RetryCount,
                    TimeoutAt = i.TimeoutAt,
                    QueueName = i.QueueName,
                    MaxRetries = i.MaxRetries
                }).ToList();

            }
        }

        public void RequeueItem(QueueItem item, int retryCount = 5)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                var dbItem = dbSet.Single(i => i.Id.Equals(item.Id));

                dbItem.CreatedAt = item.CreatedAt;
                dbItem.Payload = item.Payload;
                dbItem.RetryCount = retryCount;
                dbItem.Status = QueueStatus.Queued;
                dbItem.TimeoutAt = null;
                db.SaveChanges();
            }
        }

        public void MoveToDeadMessageQueue(QueueItem item)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                var deadLetterDbSet = _deadLetterDbSetProvider(db);
                var dbItem = dbSet.Single(i => i.Id.Equals(item.Id));

                dbSet.Remove(dbItem);
                deadLetterDbSet.Add(new DeadLetter(item));
                db.SaveChanges();
            }
        }

        public void Purge(QueueItem item)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                var dbItem = dbSet.Single(i => i.Id.Equals(item.Id));

                dbSet.Remove(dbItem);

                db.SaveChanges();
            }
        }
    }
}
