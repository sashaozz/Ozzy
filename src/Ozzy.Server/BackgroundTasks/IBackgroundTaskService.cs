using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.BackgroundTasks
{
    public interface IBackgroundTaskService
    {
        void AddBackgroundTask(string code);
        BackgroundTaskRecord GetNextTask();
        void RemoveTask(string code);
    }
}
