using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.BackgroundTasks
{
    public interface ITaskQueueService
    {
        void AddBackgroundTask<T>() where T : BaseBackgroundTask;
        BaseBackgroundTask FetchNextTask();
        void RemoveTask(string code);
    }
}
