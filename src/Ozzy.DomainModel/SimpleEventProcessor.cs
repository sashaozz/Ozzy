using System;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Простой процессор событий, который сохраняет текущую позицию обработанных записей в памяти и использует делегат для обработки событий
    /// </summary>
    public class SimpleEventProcessor : IDomainEventsProcessor
    {
        private long _checkpoint;
        private readonly Action<DomainEventRecord> _processor;
        public SimpleEventProcessor(long checkpoint, Action<DomainEventRecord> processor)
        {
            _checkpoint = checkpoint;
            if (processor == null) throw new ArgumentNullException(nameof(processor));            
            _processor = processor;
        }

        public void OnNext(DomainEventEntry data, long sequence, bool endOfBatch)
        {
            _processor(data.Value);
            _checkpoint++;
        }

        public long GetCheckpoint()
        {
            return _checkpoint;
        }
    }
}
