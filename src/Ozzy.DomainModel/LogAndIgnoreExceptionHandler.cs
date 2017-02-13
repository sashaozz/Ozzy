using System;
using Disruptor;
using Ozzy.Core;

namespace Ozzy.DomainModel
{
    public class LogAndIgnoreExceptionHandler : IExceptionHandler
    {
        public void HandleEventException(Exception ex, long sequence, object evt)
        {
            OzzyLogger<IDomainModelTracing>.Log.ProcessDomainEventEntryException(evt as DomainEventEntry, sequence, ex);
        }

        public void HandleOnStartException(Exception ex)
        {
            OzzyLogger<IDomainModelTracing>.Log.EventsProcessorStartException(ex);
        }

        public void HandleOnShutdownException(Exception ex)
        {
            OzzyLogger<IDomainModelTracing>.Log.EventsProcessorStopException(ex);
        }
    }
}
