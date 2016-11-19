using System;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Базовый класс для процессоров доменных событий, который использует ICheckpointManager для контроля прогресса обработки очереди событий
    /// </summary>
    public abstract class BaseEventsProcessor : IDomainEventsProcessor
    {
        protected ICheckpointManager CheckpointManager { get; set; }

        /// <summary>
        /// Конструктор обработчика доменных событий
        /// </summary>
        /// <param name="checkpointManager">Менеджер контрольных точек, который используется для получения и сохранения
        /// текущей записи в очереди доменных событий, обработанной данным обработчиком</param>        
        protected BaseEventsProcessor(ICheckpointManager checkpointManager)
        {
            CheckpointManager = checkpointManager;
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
                Logger<IDomainModelTracing>.Log.TraceVerboseEvent($"Processing empty domain record at sequence {sequence}");
                CheckpointManager.SaveCheckpoint(sequence);
                return;
            }
            try
            {
               ProcessEvent(data);
            }
            catch (Exception e)
            {
                HandleException(data, sequence, e);
            }
            CheckpointManager.SaveCheckpoint(sequence);
        }

        protected abstract void ProcessEvent(DomainEventEntry data);
        protected virtual void HandleException(DomainEventEntry data, long sequence, Exception exception)
        {
            Logger<IDomainModelTracing>.Log.ProcessDomainEventEntryException(data, sequence, exception);
        }

        public long GetCheckpoint()
        {
            return CheckpointManager.GetCheckpoint();
        }        
    }
}
