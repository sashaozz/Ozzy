using System.Linq;

namespace Ozzy.DomainModel
{
    public interface IDataRepository<TItem, TId> where TItem : class, IEntity<TId>
    {
        IQueryable<TItem> Query();
        void Create(TItem item);
        void Remove(TItem item);
        void Remove(TId id);
        void Update(TItem item);
    }
}
