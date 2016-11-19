﻿using System.Collections.Generic;

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
            Events = _events = new List<IDomainEvent>();
        }

        /// <summary>
        /// Конструктор по умолчанию для ORM
        /// </summary>
        protected AggregateBase() : base()
        {
            Events = _events = new List<IDomainEvent>();
        }        

        /// <summary>
        /// Список доменных событий, связанных с агрегатом.
        /// </summary>
        public List<IDomainEvent> Events { get; protected set; }

        /// <summary>
        /// Создает новое доменное событие в данном агрегате.
        /// </summary>
        /// <typeparam name="TEvent">Тип доменного события</typeparam>
        /// <param name="event">Доменное событие</param>
        public void RaiseEvent<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            _events.Add(@event);
        }
    }
}