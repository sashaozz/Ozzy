using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ozzy.Server.EntityFramework
{
    public class EfQueueRepository : IQueueRepository
    {
        private Func<AggregateDbContext> _dbFactory;
        private Func<AggregateDbContext, DbSet<QueueRecord>> _dbSetProvider;

        public EfQueueRepository(Func<AggregateDbContext> dbFactory, Func<AggregateDbContext, DbSet<QueueRecord>> dbSetProvider)
        {
            Guard.ArgumentNotNull(dbFactory, nameof(dbFactory));
            Guard.ArgumentNotNull(dbSetProvider, nameof(dbSetProvider));
            _dbFactory = dbFactory;
            _dbSetProvider = dbSetProvider;

        }        

        public string Put(string queueName, byte[] item)
        {
            var record = new QueueRecord()
            {
                Payload = item,
                QueueName = queueName                
            };
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                dbSet.Add(record);
                db.SaveChanges();
            }
            return record.Id;
        }

        public QueueItem Fetch(string queueName)
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
                item.Status = QueueStatus.Fetched;
                item.FetchedAt = DateTime.UtcNow;

                db.SaveChanges();
                return new QueueItem(item.Id, item.Payload)
                {
                    CreatedAt = item.CreatedAt,
                    FetchedAt = item.FetchedAt,
                    RetryCount = item.RetryCount
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

        public List<QueueItem> GetFetched(string queueName)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);

                var items = dbSet
                    .Where(s => s.QueueName == queueName)
                    .Where(s => s.Status == QueueStatus.Fetched)
                    .OrderBy(i => i.CreatedAt)
                    .ToList();

                return items.Select(i => new QueueItem(i.Id, i.Payload)
                {
                    CreatedAt = i.CreatedAt,
                    FetchedAt = i.FetchedAt,
                    RetryCount = i.RetryCount
                }).ToList();

            }
        }

        public void RequeueItem(string queueName, QueueItem item)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                var dbItem = dbSet.Single(i => i.Id.Equals(item.Id));

                dbItem.CreatedAt = item.CreatedAt;
                dbItem.FetchedAt = item.FetchedAt;
                dbItem.Payload = item.Payload;
                dbItem.RetryCount = item.RetryCount;
                dbItem.Status = QueueStatus.Queued;

                db.SaveChanges();
            }
        }
    }
}
