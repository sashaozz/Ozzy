using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Базовый класс для всех агрегатов
    /// </summary>
    public abstract class AggregateBase<T> : EntityBase<T>, IAggregate<T>
    {

        private readonly List<IDomainEvent> _events;

        /// <summary>
        /// Создает агрегат с указанными Id
        /// </summary>
        /// <param name="id"></param>
        protected AggregateBase(T id) : base(id)
        {
            //Events = 
            _events = new List<IDomainEvent>();
        }

        /// <summary>
        /// Конструктор по умолчанию для ORM
        /// </summary>
        protected AggregateBase() : base()
        {
            //Events = 
            _events = new List<IDomainEvent>();
        }

        /// <summary>
        /// Список доменных событий, связанных с агрегатом.
        /// </summary>
        //[JsonIgnore]
        //public List<IDomainEvent> Events { get; protected set; }
        public IEnumerable<IDomainEvent> GetUndispatchedEvents()
        {
            return _events;
        }

        public void ClearUndispatchedEvents()
        {
            _events.Clear();
        }

        /// <summary>
        /// Создает новое доменное событие в данном агрегате.
        /// </summary>
        /// <typeparam name="TEvent">Тип доменного события</typeparam>
        /// <param name="event">Доменное событие</param>
        protected void RaiseEvent<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            _events.Add(@event);
        }
    }
}