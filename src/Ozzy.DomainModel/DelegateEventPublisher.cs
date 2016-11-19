using System;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Реализация IFastEventPublisher, которая публикует доменные события с помощью делегата
    /// </summary>
    public class DelegateEventPublisher : IFastEventPublisher
    {
        private readonly Action<DomainEventRecord> _publishAction;
        public DelegateEventPublisher(Action<DomainEventRecord> publishAction)
        {
            if (publishAction == null) throw new ArgumentNullException(nameof(publishAction));
            _publishAction = publishAction;
        }

        public void Publish(DomainEventRecord message)
        {
            _publishAction(message);
        }
    }
}
