using System;
using Ozzy.Core;
using System.Collections.Generic;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Базовый класс для процессоров доменных событий, который использует ICheckpointManager для контроля прогресса обработки очереди событий
    /// </summary>
    public abstract class BaseEventsProcessor : IDomainEventsProcessor
    {
        protected ICheckpointManager CheckpointManager { get; set; }
        protected Dictionary<Type, Action<object>> Handlers { get; set; } = new Dictionary<Type, Action<object>>();

        /// <summary>
        /// Конструктор обработчика доменных событий
        /// </summary>
        /// <param name="checkpointManager">Менеджер контрольных точек, который используется для получения и сохранения
        /// текущей записи в очереди доменных событий, обработанной данным обработчиком</param>        
        protected BaseEventsProcessor(ICheckpointManager checkpointManager)
        {
            CheckpointManager = checkpointManager;// ?? new InMemoryCheckpointManager();
        }

        protected void AddHandler<T>(Action<T> handler) where T : class, IDomainEvent
        {
            Handlers.Add(typeof(T), message => handler(message as T));
        }

        /// <summary>
        /// Обрабатывает следующую запись из очереди доменных событий
        /// </summary>
        /// <param name="data">Запись, содержащая информацию о доменной событии</param>
        /// <param name="sequence">Глобальный номер события в очереди доменных событий</param>
        /// <param name="endOfBatch">Является ли последним событием в группе</param>

        public void OnNext(DomainEventEntry data, long sequence, bool endOfBatch)
        {
            if (data?.Value == null || data.Value is EmptyEventRecord)
            {
                OzzyLogger<IDomainModelTracing>.Log.TraceVerboseEvent($"Processing empty domain record at sequence {sequence}");
                CheckpointManager.SaveCheckpoint(sequence);
                return;
            }
            try
            {
                OzzyLogger<IDomainModelTracing>.Log.ProcessDomainEventEntry(data);
                ProcessEvent(data);
            }
            catch (Exception e)
            {
                HandleException(data, sequence, e);
            }
            CheckpointManager.SaveCheckpoint(sequence);
        }

        public void OnEvent(DomainEventEntry record)
        {
            ProcessEvent(record);
        }

        protected virtual void ProcessEvent(DomainEventEntry record)
        {
            HandleEvent(record.Value);
        }

        protected virtual void HandleEvent(DomainEventRecord record)
        {
            var t = record.GetDomainEventType();
            var handler = Handlers.GetValueOrDefault(t);
            handler?.Invoke(record.GetDomainEvent());
        }

        protected virtual void HandleException(DomainEventEntry data, long sequence, Exception exception)
        {
            OzzyLogger<IDomainModelTracing>.Log.ProcessDomainEventEntryException(data, sequence, exception);
        }

        public long GetCheckpoint()
        {
            return CheckpointManager.GetCheckpoint();
        }


    }
}
