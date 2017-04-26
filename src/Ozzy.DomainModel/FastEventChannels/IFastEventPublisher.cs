namespace Ozzy.DomainModel
{
    /// <summary>
    /// Канал для "быстрой" публикации доменных событий для подписчиков.
    /// Данный канал может не гарантировать доставку сообещений (пропавшие сообщения всё-равно будут доставлены через основной "медленный" канал).
    /// Например можно использовать неперсистентные in-memory очереди, Redis pub-sub и т.д.
    /// </summary>
    public interface IFastEventPublisher
    {
        /// <summary>
        /// Публикует доменное событие через "быстрый" канал доставки сообщений
        /// </summary>
        /// <param name="message">Событие для отправки</param>
        void Publish(IDomainEventRecord message);
    }    
}
