using System.Collections.Generic;

namespace Ozzy.DomainModel
{
    public interface IAggregate<T> : IAggregate, IEntity<T>
    {        
    }

    /// <summary>
    /// Интерфей для всех агрегатов доменной модели
    /// </summary>
    public interface IAggregate : IEntity
    {
        /// <summary>
        /// Список доменных событий, произошедших в агрегате
        /// </summary>
        //List<IDomainEvent> Events { get; }
        IEnumerable<IDomainEvent> GetUndispatchedEvents();
        void ClearUndispatchedEvents();
    }
}
