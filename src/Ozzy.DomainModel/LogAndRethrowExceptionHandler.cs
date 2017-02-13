using System;
using Disruptor;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    public class LogAndRethrowExceptionHandler : IExceptionHandler
    {
        public void HandleEventException(Exception ex, long sequence, object evt)
        {
            OzzyLogger<IDomainModelTracing>.Log.ProcessDomainEventEntryException(evt as DomainEventEntry, sequence, ex);
            OzzyLogger<IDomainModelTracing>.Log.TraceCriticalEvent($"Обработка события заершилось исключением {ex}. Обработка событий данным обработчиком удет остановлена. Необходимо устранить причину ошибки и перезапустить обработчики.");
            throw ex;
        }

        public void HandleOnStartException(Exception ex)
        {
            OzzyLogger<IDomainModelTracing>.Log.EventsProcessorStartException(ex);
            OzzyLogger<IDomainModelTracing>.Log.TraceCriticalEvent($"Старт обаботчика событий заершился исключением {ex}. Обработка событий данным обработчиком удет остановлена. Необходимо устранить причину ошибки и перезапустить обработчики.");
            throw ex;
        }

        public void HandleOnShutdownException(Exception ex)
        {
            OzzyLogger<IDomainModelTracing>.Log.EventsProcessorStopException(ex);           
            throw ex;
        }
    }
}
