using System;
using Ozzy.Core;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Базовый класс для процессоров доменных событий, который использует ICheckpointManager для контроля прогресса обработки очереди событий
    /// </summary>
    public class DomainEventsProcessor : IDomainEventsProcessor
    {
        //private IDomainEventHandler _handler;
        private Func<DomainEventRecord, bool> _handler;

        protected ICheckpointManager CheckpointManager { get; set; }
        protected Dictionary<Type, Func<object, bool>> Handlers { get; set; } = new Dictionary<Type, Func<object, bool>>();

        /// <summary>
        /// Конструктор обработчика доменных событий
        /// </summary>
        /// <param name="checkpointManager">Менеджер контрольных точек, который используется для получения и сохранения
        /// текущей записи в очереди доменных событий, обработанной данным обработчиком</param>        
        protected DomainEventsProcessor(ICheckpointManager checkpointManager)
        {
            _handler = HandleEvent;
            CheckpointManager = checkpointManager;// ?? new InMemoryCheckpointManager();

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

        public DomainEventsProcessor(IDomainEventHandler handler, ICheckpointManager checkpointManager)
        {
            _handler = handler.HandleEvent;
            CheckpointManager = checkpointManager;// ?? new InMemoryCheckpointManager();
        }

        /// <summary>
        /// Обрабатывает следующую запись из очереди доменных событий
        /// </summary>
        /// <param name="data">Запись, содержащая информацию о доменной событии</param>
        /// <param name="sequence">Глобальный номер события в очереди доменных событий</param>
        /// <param name="endOfBatch">Является ли последним событием в группе</param>
        public void OnNext(DomainEventEntry record, long sequence, bool endOfBatch)
        {
            var data = record?.Value;
            if (data == null || data is EmptyEventRecord)
            {
                OzzyLogger<IDomainModelTracing>.Log.TraceVerboseEvent($"Processing empty domain record at sequence {sequence}");
                CheckpointManager.SaveCheckpoint(sequence, true);
                return;
            }
            OzzyLogger<IDomainModelTracing>.Log.ProcessDomainEventEntry(record);
            var isIdempotent = _handler(data);
            CheckpointManager.SaveCheckpoint(sequence, isIdempotent);
        }
        public virtual bool CanHandleMessage(Type messageType)
        {
            return Handlers.ContainsKey(messageType);
        }
        protected virtual bool HandleEvent(DomainEventRecord record)
        {
            var messageType = record.GetDomainEventType();
            if (!CanHandleMessage(messageType)) return true;
            var message = record.GetDomainEvent();
            var handler = Handlers.GetValueOrDefault(messageType);
            return handler.Invoke(message);
        }
        public long GetCheckpoint()
        {
            return CheckpointManager.GetCheckpoint();
        }
    }
}
