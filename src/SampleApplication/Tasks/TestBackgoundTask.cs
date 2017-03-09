using Ozzy.Server.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleApplication.Tasks
{
    public class TestBackgoundTask : BaseBackgroundTask
    {
        public override async Task Execute()
        {
            Thread.Sleep(2000);
        }
    }
}
