namespace Ozzy.DomainModel
{
    /// <summary>
    /// Реализация IFastEventPublisher, которая не публикует события никуда.
    /// </summary>
    public class NullEventsPublisher : IFastEventPublisher
    {
        public static NullEventsPublisher Instance = new NullEventsPublisher();
        public void Publish(IDomainEventRecord message)
        {
            //do nothing
        }
    }
}
