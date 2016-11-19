using System;
using Disruptor;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    public class LogAndRethrowExceptionHandler : IExceptionHandler
    {
        public void HandleEventException(Exception ex, long sequence, object evt)
        {
            Logger<IDomainModelTracing>.Log.ProcessDomainEventEntryException(evt as DomainEventEntry, sequence, ex);
            Logger<IDomainModelTracing>.Log.TraceCriticalEvent($"Обработка события заершилось исключением {ex}. Обработка событий данным обработчиком удет остановлена. Необходимо устранить причину ошибки и перезапустить обработчики.");
            throw ex;
        }

        public void HandleOnStartException(Exception ex)
        {
            Logger<IDomainModelTracing>.Log.EventsProcessorStartException(ex);
            Logger<IDomainModelTracing>.Log.TraceCriticalEvent($"Старт обаботчика событий заершился исключением {ex}. Обработка событий данным обработчиком удет остановлена. Необходимо устранить причину ошибки и перезапустить обработчики.");
            throw ex;
        }

        public void HandleOnShutdownException(Exception ex)
        {
            Logger<IDomainModelTracing>.Log.EventsProcessorStopException(ex);           
            throw ex;
        }
    }
}
