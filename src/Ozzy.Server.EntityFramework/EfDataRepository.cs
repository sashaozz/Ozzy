using Microsoft.EntityFrameworkCore;
using Ozzy.DomainModel;
using System;
using System.Linq;

namespace Ozzy.Server.EntityFramework
{
    public class EfDataRepository<TItem, TId> : IDataRepository<TItem,TId> where TItem : GenericDataRecord<TId>
    {
        private TransientAggregateDbContext _db;
        private Func<AggregateDbContext, DbSet<TItem>> _dbSetProvider;

        public EfDataRepository(TransientAggregateDbContext db, Func<AggregateDbContext, DbSet<TItem>> dbSetProvider)
        {
            _db = db;
            _dbSetProvider = dbSetProvider;

        }
        public void Create(TItem item)
        {
            using (var db = _db.Clone())
            {
                var dbSet = _dbSetProvider(db);
                dbSet.Add(item);
                db.AddDomainEvent(new DataRecordCreatedEvent()
                {
                    RecordType = item.GetType(),
                    RecordValue = item
                });
                db.SaveChanges();
            }
        }

        public IQueryable<TItem> Query()
        {
            return _dbSetProvider(_db).AsNoTracking();
        }

        public void Remove(TItem item)
        {
            using (var db = _db.Clone())
            {
                var dbSet = _dbSetProvider(db);
                dbSet.Remove(item);
                db.SaveChanges();
            }
        }

        public void Remove(TId id)
        {
            using (var db = _db.Clone())
            {
                var dbSet = _dbSetProvider(db);
                var item = dbSet.Single(i => i.Id.Equals(id));
                dbSet.Remove(item);
                db.SaveChanges();
            }
        }

        public void Update(TItem item)
        {
            using (var db = _db.Clone())
            { 
                var dbSet = _dbSetProvider(db);
                var existingItem = dbSet.Find(item.Id);
                //if (item.Version <= existingItem.Version)
                //{
                //    throw new InvalidOperationException("version is too low");
                //}
                db.Entry(existingItem).CurrentValues.SetValues(item);
                //db.Entry(existingItem).State = EntityState.Detached;
                //dbSet.Attach(item);
                //db.Entry(item).State = EntityState.Modified;
                //db.Entry(item).Property(i => i.Version).OriginalValue = existingItem.Version;
                
                db.SaveChanges();
            }
        }
        protected virtual TItem UpdateItem(TItem existingItem, TItem updatedItem)
        {
            return updatedItem;
        }
    }
}
