
using Ozzy.Server;

namespace Ozzy.DomainModel
{
    public interface IFastEventRecieverFactory
    {
        IFastEventReciever CreateReciever();// where TLoop : DomainEventsManager;
    }

    public interface IFastEventRecieverFactory<TDomain>// : IFastEventRecieverFactory
        where TDomain : IOzzyDomainModel
    {
        IFastEventReciever<TLoop> CreateReciever<TLoop>() where TLoop : DomainEventLoop<TDomain>;
    }
}