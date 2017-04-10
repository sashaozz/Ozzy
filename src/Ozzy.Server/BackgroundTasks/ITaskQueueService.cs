using Ozzy.Server.Queues;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.BackgroundTasks
{
    public interface ITaskQueueService: IQueueService<BaseBackgroundTask>
    {
        void Add<T>(string configuration = null) where T : BaseBackgroundTask;
    }
}
