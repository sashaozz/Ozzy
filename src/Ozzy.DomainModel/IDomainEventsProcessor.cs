using Disruptor;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Процессор очереди доменных событий
    /// </summary>
    public interface IDomainEventsProcessor : IEventHandler<DomainEventEntry>, IWorkHandler<DomainEventEntry>
    {
        /// <summary>
        /// Получение текущий позиции в очереди доменных событий на которой находитс процессор
        /// </summary>
        /// <returns></returns>
        long GetCheckpoint();
    }
}