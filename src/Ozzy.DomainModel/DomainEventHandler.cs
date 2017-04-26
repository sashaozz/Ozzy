using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    public class DomainEventsHandler : IDomainEventsHandler
    {
        private Func<IDomainEventRecord, bool> _handler;
        protected Dictionary<Type, Func<object, bool>> Handlers { get; set; } = new Dictionary<Type, Func<object, bool>>();
        protected DomainEventsHandler()
        {
            _handler = HandleEvent;

            var interfaces = this.GetType().GetTypeInfo().GetInterfaces();
            var handleType = typeof(IHandleEvent<>);
            var reflectedRegisterMethod = this.GetType().GetTypeInfo().GetMethod("RegisterHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var type in interfaces)
            {
                if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == handleType)
                {
                    var messageType = type.GetTypeInfo().GetGenericArguments()[0];
                    var handleMethod = this.GetType().GetTypeInfo().GetMethod("Dispatch", BindingFlags.NonPublic | BindingFlags.Instance)
                        .MakeGenericMethod(messageType);

                    var instance = Expression.Constant(this, this.GetType());
                    var messageParam = Expression.Parameter(typeof(object), "message");

                    var lambdaExpression = Expression.Lambda(Expression.Call(instance, handleMethod, messageParam), messageParam);
                    var compiledLambdaExpression = lambdaExpression.Compile();

                    var reflectedRegisterMethodWithParameter = reflectedRegisterMethod.MakeGenericMethod(messageType);
                    reflectedRegisterMethodWithParameter.Invoke(this, new[] { compiledLambdaExpression });
                }
            }
        }

        protected void RegisterHandler<TMessage>(Func<object, bool> handler)
        {
            Handlers.Add(typeof(TMessage), handler);
        }

        protected bool Dispatch<TMessage>(object message)
        {
            var handler = this as IHandleEvent<TMessage>;
            TMessage data = (TMessage)message;
            return handler.Handle(data);
        }
        public virtual bool CanHandleMessage(Type messageType)
        {
            return Handlers.ContainsKey(messageType);
        }
        public virtual bool HandleEvent(IDomainEventRecord record)
        {
            var messageType = record.GetDomainEventType();
            if (!CanHandleMessage(messageType)) return true;
            var message = record.GetDomainEvent();
            var handler = Handlers.GetValueOrDefault(messageType);
            return handler.Invoke(message);
        }
    }
}
