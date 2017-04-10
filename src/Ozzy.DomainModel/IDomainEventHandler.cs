namespace Ozzy.DomainModel
{
    public interface IDomainEventHandler
    {
        bool HandleEvent(DomainEventRecord record);
    }
}
