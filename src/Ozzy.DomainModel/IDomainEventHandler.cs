namespace Ozzy.DomainModel
{
    /// <summary>
    /// Обработчик доменного события
    /// </summary>
    public interface IDomainEventHandler
    {
        /// <summary>
        /// Обрабатывает доменное событие
        /// </summary>
        /// <param name="record"></param>
        void HandleEvent(DomainEventRecord record);
    }
}