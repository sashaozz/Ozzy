using Ozzy.Core;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Процессор доменных событий, который использует IDomainEventHandler для обработки 
    /// событий и ICheckpointManager для контроля прогресса обработки очереди событий
    /// </summary>
    public class EventsProcessor : BaseEventsProcessor
    {
        protected IDomainEventHandler Handler;
        

        /// <summary>
        /// Конструктор обработчика доменных событий
        /// </summary>
        /// <param name="checkpointManager">Менеджер контрольных точек, который используется для получения и сохранения
        /// текущей записи в очереди доменных событий, обработанной данным обработчиком</param>
        /// <param name="handler"></param>
        public EventsProcessor(IDomainEventHandler handler, ICheckpointManager checkpointManager) : base(checkpointManager)
        {
            Handler = handler;
        }        

        protected override void ProcessEvent(DomainEventEntry data)
        {
            Logger<IDomainModelTracing>.Log.ProcessDomainEventEntry(data);
            Handler.HandleEvent(data.Value);
        }   
    }
}
