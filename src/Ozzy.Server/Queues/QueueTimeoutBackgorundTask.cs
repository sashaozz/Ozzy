using Ozzy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace Ozzy.Server.Queues
{
    public class QueueTimeoutBackgroundProcess : Server.SingleInstanceProcess<QueueTimeoutBackgroundAction>
    {

        public QueueTimeoutBackgroundProcess(IDistributedLockService lockService, QueuesFaultManager queuesFaultManager)
            : base(lockService, new QueueTimeoutBackgroundAction(queuesFaultManager))
        {

        }
    }

    public class QueueTimeoutBackgroundAction : PeriodicActionProcess, IBackgroundProcess
    {
        QueuesFaultManager _queuesFaultManager;
        public QueueTimeoutBackgroundAction(QueuesFaultManager queuesFaultManager)
        {
            _queuesFaultManager = queuesFaultManager;
        }

        protected override Task ActionAsync(CancellationToken cts)
        {
            _queuesFaultManager.ProcessTimeoutedItems();
            return Task.CompletedTask;
        }
    }
}
