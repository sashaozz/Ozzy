using Ozzy.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.BackgroundTasks
{
    public interface IBackgroundTaskRepository : IDataRepository<BackgroundTaskRecord, string>
    {
        BackgroundTaskRecord FetchNextTask();
    }
}
