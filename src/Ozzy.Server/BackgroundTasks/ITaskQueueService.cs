using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.BackgroundTasks
{
    public interface ITaskQueueService
    {
        void AddBackgroundTask(string code);
        BackgroundTaskRecord FetchNextTask();
        void RemoveTask(string code);
    }
}
