using System;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Базовый класс для процессоров доменных событий, который использует ICheckpointManager для контроля прогресса обработки очереди событий
    /// </summary>
    public class DomainEventsProcessor : IDomainEventsProcessor
    {
        private Func<IDomainEventRecord, bool> _handler;
        protected ICheckpointManager CheckpointManager { get; set; }
        public DomainEventsProcessor(IDomainEventsHandler handler, ICheckpointManager checkpointManager)
        {
            _handler = handler.HandleEvent;
            CheckpointManager = checkpointManager; // ?? new InMemoryCheckpointManager();
        }

        protected DomainEventsProcessor(ICheckpointManager checkpointManager)
        {
            _handler = HandleEvent;
            CheckpointManager = checkpointManager; // ?? new InMemoryCheckpointManager();
        }

        public virtual bool HandleEvent(IDomainEventRecord record)
        {
            return false;
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
        public long GetCheckpoint()
        {
            return CheckpointManager.GetCheckpoint();
        }
    }
}
