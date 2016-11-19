using System.Collections.Generic;

namespace Ozzy.DomainModel
{
    public interface IAggregate<T> : IAggregate, IEntity<T>
    {        
    }

    /// <summary>
    /// Интерфей для всех агрегатов доменной модели
    /// </summary>
    public interface IAggregate
    {       
        /// <summary>
        /// Список доменных событий, произошедших в агрегате
        /// </summary>
        List<IDomainEvent> Events { get; }
        /// <summary>
        /// Создает доменное событие в агрегате
        /// </summary>
        /// <typeparam name="TEvent">Тип доменного события</typeparam>
        /// <param name="event">Доменное событие</param>
        void RaiseEvent<TEvent>(TEvent @event) where TEvent : IDomainEvent;
    }
}
