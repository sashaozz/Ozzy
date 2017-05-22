﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Ozzy.Core;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public class SagaDomainEventsHandler<TSaga> : IDomainEventsHandler
        where TSaga : SagaBase
    {
        private Func<IDomainEventRecord, bool> _handler;
        private Dictionary<Type, Func<TSaga, object, bool>> Handlers { get; set; } = new Dictionary<Type, Func<TSaga, object, bool>>();
        private ISagaRepository _sagaRepository;
        public Type SagaType = typeof(TSaga);

        public SagaDomainEventsHandler(ISagaRepository sagaRepository)
        {
            _handler = HandleEvent;
            _sagaRepository = sagaRepository;

            var interfaces = SagaType.GetTypeInfo().GetInterfaces();
            var handleType = typeof(IHandleEvent<>);
            var reflectedRegisterMethod = this.GetType().GetTypeInfo().GetMethod("RegisterSagaHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var type in interfaces)
            {
                if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == handleType)
                {
                    var messageType = type.GetGenericArguments()[0];
                    var handleMethod = this.GetType().GetTypeInfo()
                        .GetMethod("DispatchEventToSaga", BindingFlags.NonPublic | BindingFlags.Instance)
                        .MakeGenericMethod(messageType);

                    var instance = Expression.Constant(this, this.GetType());
                    var sagaParam = Expression.Parameter(typeof(TSaga), "saga");
                    var messageParam = Expression.Parameter(typeof(object), "message");

                    var lambdaExpression = Expression.Lambda(Expression.Call(instance, handleMethod, sagaParam, messageParam), sagaParam, messageParam);
                    var compiledLambdaExpression = lambdaExpression.Compile();

                    var reflectedRegisterMethodWithParameter = reflectedRegisterMethod.MakeGenericMethod(messageType);
                    reflectedRegisterMethodWithParameter.Invoke(this, new[] { compiledLambdaExpression });
                }
            }
        }

        private void RegisterSagaHandler<TMessage>(Func<TSaga, object, bool> handler)
        {
            this.Handlers.Add(typeof(TMessage), handler);
        }

        private bool DispatchEventToSaga<TMessage>(TSaga saga, object message)
        {
            var handler = saga as IHandleEvent<TMessage>;
            return handler.Handle((TMessage)message);
        }

        public bool CanHandleMessage(Type messageType)
        {
            return Handlers.ContainsKey(messageType);
        }

        public bool HandleEvent(IDomainEventRecord record)
        {
            var messageType = record.GetDomainEventType();
            if (!CanHandleMessage(messageType)) return true;

            var message = record.GetDomainEvent();
            var saga = (message is SagaCommand) ?
                _sagaRepository.GetSagaById<TSaga>((message as SagaCommand).SagaId)
                : _sagaRepository.CreateNewSaga<TSaga>();
            var handler = Handlers.GetValueOrDefault(messageType);
            var idempotent = handler.Invoke(saga, message);
            saga.SagaState.SagaVersion++;
            //todo : better handle transient faults
            _sagaRepository.SaveSaga(saga);
            return idempotent;
        }
    }
}