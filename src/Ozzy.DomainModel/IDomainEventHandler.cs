namespace Ozzy.DomainModel
{
    public interface IDomainEventsHandler
    {
        bool HandleEvent(IDomainEventRecord record);
    }
}
