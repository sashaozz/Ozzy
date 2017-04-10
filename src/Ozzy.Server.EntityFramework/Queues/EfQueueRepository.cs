using Microsoft.EntityFrameworkCore;
using Ozzy.Core;
using Ozzy.DomainModel;
using Ozzy.DomainModel.Queues;
using Ozzy.Server.Queues;
using System;
using System.Linq;

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

        public virtual void Acknowledge(string id)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                var dbItem = dbSet.Single(i => i.Id.Equals(id));
                dbSet.Remove(dbItem);
                db.SaveChanges();
            }
        }

        public virtual void Create(QueueRecord item)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                dbSet.Add(item);
                db.AddDomainEvent(new DataRecordCreatedEvent<QueueRecord>()
                {
                    RecordType = item.GetType(),
                    RecordValue = item
                });
                db.SaveChanges();
            }
        }

        public virtual QueueRecord FetchNext(string queueName)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);

                var item = dbSet
                    .Where(s => s.QueueName == queueName)
                    .Where(s => s.Status == QueueStatus.Awaiting)
                    .OrderBy(i => i.CreatedAt)
                    .FirstOrDefault();

                item.Status = QueueStatus.Processing;

                db.SaveChanges();
                return item;
            }
        }

        public virtual IQueryable<QueueRecord> Query()
        {
            return _dbSetProvider(_dbFactory()).AsNoTracking();
        }

    }
}
