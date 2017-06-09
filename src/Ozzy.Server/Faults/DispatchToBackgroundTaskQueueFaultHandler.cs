using System;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public class DispatchToBackgroundTaskQueueFaultHandler : IDomainEventsFaultHandler
    {
        private BackgroundTaskQueue _backgroundJobQueue;
        private JobQueue<FaultInfo> _faultQueueService;

        public DispatchToBackgroundTaskQueueFaultHandler(BackgroundTaskQueue backgroundJobQueue, JobQueue<FaultInfo> faultQueueService)
        {
            _backgroundJobQueue = backgroundJobQueue;
            _faultQueueService = faultQueueService;
        }

        public void Handle(Type processorType, IDomainEventRecord record, int retryMaxCount = 5, bool sendToErrorQueue = true)
        {
            var retries = 0;// record.MetaData.ContainsKey("retries") ? (Int64)record.MetaData["retries"] : 0;

            if (retries >= retryMaxCount)
            {
                //if (sendToErrorQueue)
                //    _faultQueueService.Put(new FaultInfo()
                //    {
                //        ProcessorType = processorType,
                //        Record = record,
                //        RetryMaxCount = retryMaxCount
                //    });

                return;
            }

            //record.MetaData["retries"] = ++retries;
            try
            {
                _backgroundJobQueue.PutJob<RetryEventTask, RetryEventTaskParams>(new RetryEventTaskParams()
                {
                    ProcessorType = processorType.AssemblyQualifiedName,
                    Record = new Test(record.GetDomainEvent(), record.GetDomainEventType(), record.Sequence),
                    RetryMaxCount = retryMaxCount,
                    sendToErrorQueue = sendToErrorQueue
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
