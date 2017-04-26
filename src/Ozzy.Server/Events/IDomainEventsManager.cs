using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public interface IDomainEventsManager
    {
        void AddDomainEvent(IDomainEvent domainEvent);
    }
}
