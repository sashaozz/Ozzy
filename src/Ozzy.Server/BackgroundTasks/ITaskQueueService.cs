using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.BackgroundTasks
{
    public interface ITaskQueueService
    {
        void AddBackgroundTask<T>(string configuration = null) where T : BaseBackgroundTask;
        BaseBackgroundTask FetchNextTask();
        void AcknowledgeTask(string code);
    }
}
