using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ozzy.DomainModel;
using Ozzy.Server.BackgroundTasks;
using Ozzy.Server.Queues;

namespace Ozzy.Server.Faults
{
    public class FaultManager : IFaultManager
    {
        ITaskQueueService _taskQueueService;

        public FaultManager(ITaskQueueService taskQueueService, IQueueService<FaultInfo> faultQueueService)
        {
            _taskQueueService = taskQueueService;
        }

        public void Handle(Type processorType, DomainEventRecord record, int retryMaxCount = 5, bool sendToErrorQueue = true)
        {
            var retries = record.MetaData.ContainsKey("retries") ? (int)record.MetaData["retries"] : 0;

            if (retries >= retryMaxCount)
            {
                if(sendToErrorQueue)

                return;
            }

            record.MetaData["retries"] = ++retries;

            _taskQueueService.Add<RetryEventTask, RetryEventTaskParams>(new RetryEventTaskParams()
            {
                ProcessorType = processorType,
                Record = record,
                RetryMaxCount = retryMaxCount,
                sendToErrorQueue = sendToErrorQueue
            });
        }
    }
}
