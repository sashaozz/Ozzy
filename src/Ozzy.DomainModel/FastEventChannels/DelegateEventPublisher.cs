using System;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Реализация IFastEventPublisher, которая публикует доменные события с помощью делегата
    /// </summary>
    public class DelegateEventPublisher : IFastEventPublisher
    {
        private readonly Action<IDomainEventRecord> _publishAction;
        public DelegateEventPublisher(Action<IDomainEventRecord> publishAction)
        {
            if (publishAction == null) throw new ArgumentNullException(nameof(publishAction));
            _publishAction = publishAction;
        }

        public void Publish(IDomainEventRecord message)
        {
            _publishAction(message);
        }
    }
}
