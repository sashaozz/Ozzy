using System.Collections.Generic;

namespace Ozzy.DomainModel
{
    public interface IAggregate<T> : IAggregate, IEntity<T>
    {        
    }

    /// <summary>
    /// Интерфейc для всех агрегатов доменной модели
    /// </summary>
    public interface IAggregate : IEntity
    {
        /// <summary>
        /// Список доменных событий, произошедших в агрегате
        /// </summary>
        IEnumerable<IDomainEvent> GetUndispatchedEvents();
        void ClearUndispatchedEvents();
    }
}
