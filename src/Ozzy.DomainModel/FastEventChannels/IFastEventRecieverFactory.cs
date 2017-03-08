
namespace Ozzy.DomainModel
{
    public interface IFastEventRecieverFactory
    {
        IFastEventReciever CreateReciever(DomainEventsManager loop);// where TLoop : DomainEventsManager;
    }
}