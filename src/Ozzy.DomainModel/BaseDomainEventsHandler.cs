using System;
using System.Collections.Generic;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Базовый класс обработчиков доменных событий.
    /// </summary>
    public abstract class BaseDomainEventsHandler : IDomainEventHandler
    {
        protected Dictionary<Type, Action<object>> Handlers { get; set; } = new Dictionary<Type, Action<object>>();

        public virtual void HandleEvent(DomainEventRecord record)
        {
            var t = record.GetDomainEventType();
            var handler = Handlers.GetValueOrDefault(t);
            try
            {
                handler?.Invoke(record.GetDomainEvent());
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        protected virtual void HandleException(Exception e)
        {
            Logger<ICommonEvents>.Log.Exception(e, "Exception during processing domain handler");
        }

        protected void AddHandler<T>(Action<T> handler) where T : class, IDomainEvent
        {
            Handlers.Add(typeof(T), message => handler(message as T));
        }
    }
}

