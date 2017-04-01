using Ozzy.Core;
using Ozzy.DomainModel;
using Ozzy.Server;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace Ozzy.Server.Saga
{
    public class SagaEventProcessor<TSaga, TDomain> : BaseEventsProcessor 
        where TSaga : SagaBase
        where TDomain : IOzzyDomainModel
    {
        private new Dictionary<Type, Action<TSaga, object>> Handlers { get; set; } = new Dictionary<Type, Action<TSaga, object>>();
        public Type SagaType = typeof(TSaga);
        private IServiceProvider _serviceProvider;

        public SagaEventProcessor(IExtensibleOptions<TDomain> options, IServiceProvider serviceProvider)
            : base(new SimpleChekpointManager(options.GetPersistedEventsReader()))
        {
            _serviceProvider = serviceProvider;
            var interfaces = SagaType.GetTypeInfo().GetInterfaces();
            var handleType = typeof(IHandleEvent<>);
            var reflectedRegisterMethod = this.GetType().GetTypeInfo().GetMethod("RegisterHandler", BindingFlags.NonPublic | BindingFlags.Instance);
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

        private void RegisterHandler<TMessage>(Action<TSaga, object> handler)
        {
            this.Handlers.Add(typeof(TMessage), handler);
        }

        private void DispatchEventToSaga<TMessage>(TSaga saga, object message)
        {
            var handler = saga as IHandleEvent<TMessage>;
            if (handler != null)
            {
                handler.Handle((TMessage)message);
            }
            else
            {
                //todo : throw
            }
        }

        public bool CanHandleMessage(Type messageType)
        {
            return Handlers.ContainsKey(messageType);
        }

        protected override void HandleEvent(DomainEventRecord record)
        {
            var messageType = record.GetDomainEventType();
            if (!CanHandleMessage(messageType)) return;

            var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var message = record.GetDomainEvent();
                var sagaRepository = scope.ServiceProvider.GetService<ISagaRepository>();
                var saga = (message is SagaCommand) ?
                    sagaRepository.GetSagaById<TSaga>((message as SagaCommand).SagaId)
                    : sagaRepository.CreateNewSaga<TSaga>();
                var handler = Handlers.GetValueOrDefault(messageType);
                try
                {
                    handler?.Invoke(saga, message);
                    saga.SagaState.SagaVersion++;
                    sagaRepository.SaveSaga(saga);
                }
                catch (Exception e)
                {
                    //todo handle retry
                }
            }
        }
    }
}
