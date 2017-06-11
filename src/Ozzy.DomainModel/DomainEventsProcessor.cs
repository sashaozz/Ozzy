using System;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Базовый класс для процессоров доменных событий, который использует ICheckpointManager для контроля прогресса обработки очереди событий
    /// </summary>
    public class DomainEventsProcessor : IDomainEventsProcessor
    {
        private IDomainEventsFaultHandler _faultHandler;
        private IDomainEventsHandler _handler;
        protected ICheckpointManager CheckpointManager { get; set; }
        public DomainEventsProcessor(IDomainEventsHandler handler, ICheckpointManager checkpointManager, IDomainEventsFaultHandler faultHandler = null)
        {
            Guard.ArgumentNotNull(handler, nameof(handler));
            Guard.ArgumentNotNull(checkpointManager, nameof(checkpointManager));
            if (faultHandler == null) faultHandler = new DoNothingFaultHandler();
            _handler = handler;
            _faultHandler = faultHandler;
            CheckpointManager = checkpointManager; // ?? new InMemoryCheckpointManager();
        }

        protected DomainEventsProcessor(ICheckpointManager checkpointManager)
        {
            Guard.ArgumentNotNull(checkpointManager, nameof(checkpointManager));
            CheckpointManager = checkpointManager;
            var handler = this as IDomainEventsHandler;
            _handler = handler ?? throw new InvalidOperationException("Processor should implement IDomainEventsHandler inteface to be used as Handler");
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
                //We are not saving checkpoint here because it is safe to process it again
                //CheckpointManager.SaveCheckpoint(sequence);
                return;
            }
            OzzyLogger<IDomainModelTracing>.Log.ProcessDomainEventEntry(record);
            bool isHandled = false;
            try
            {
                isHandled = _handler.HandleEvent(data);
            }
            catch (Exception e)
            {
                //todo : should we handle exception better?
                if (_faultHandler != null)
                {
                    _faultHandler.Handle(_handler.GetType(), data);
                }
            }
            // 
            if (isHandled) CheckpointManager.SaveCheckpoint(sequence);
        }
        public long GetCheckpoint()
        {
            return CheckpointManager.GetCheckpoint();
        }
    }
}
