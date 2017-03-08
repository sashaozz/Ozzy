using Microsoft.EntityFrameworkCore;
using Ozzy.Core;
using Ozzy.DomainModel;
using System;
using System.Linq;

namespace Ozzy.Server.EntityFramework
{
    public class EfDataRepository<TItem, TId> : IDataRepository<TItem,TId> where TItem : GenericDataRecord<TId>
    {
        private Func<AggregateDbContext> _dbFactory;
        private Func<AggregateDbContext, DbSet<TItem>> _dbSetProvider;

        public EfDataRepository(Func<AggregateDbContext> dbFactory, Func<AggregateDbContext, DbSet<TItem>> dbSetProvider)
        {
            Guard.ArgumentNotNull(dbFactory, nameof(dbFactory));
            Guard.ArgumentNotNull(dbSetProvider, nameof(dbSetProvider));
            _dbFactory = dbFactory;
            _dbSetProvider = dbSetProvider;

        }
        public void Create(TItem item)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                dbSet.Add(item);
                db.AddDomainEvent(new DataRecordCreatedEvent<TItem>()
                {
                    RecordType = item.GetType(),
                    RecordValue = item
                });
                db.SaveChanges();
            }
        }

        public IQueryable<TItem> Query()
        {
            return _dbSetProvider(_dbFactory()).AsNoTracking();
        }

        public void Remove(TItem item)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                dbSet.Remove(item);
                db.SaveChanges();
            }
        }

        public void Remove(TId id)
        {
            using (var db = _dbFactory())
            {
                var dbSet = _dbSetProvider(db);
                var item = dbSet.Single(i => i.Id.Equals(id));
                dbSet.Remove(item);
                db.SaveChanges();
            }
        }

        public void Update(TItem item)
        {
            using (var db = _dbFactory())
            { 
                var dbSet = _dbSetProvider(db);
                var existingItem = dbSet.Find(item.Id);
                db.Entry(existingItem).CurrentValues.SetValues(item);
                db.AddDomainEvent(new DataRecordUpdatedEvent<TItem>()
                {
                    RecordType = item.GetType(),
                    RecordValue = item
                });                

                db.SaveChanges();
            }
        }
        protected virtual TItem UpdateItem(TItem existingItem, TItem updatedItem)
        {
            return updatedItem;
        }
    }
}
