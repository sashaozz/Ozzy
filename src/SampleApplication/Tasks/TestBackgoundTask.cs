using System;
using System.Threading;
using System.Threading.Tasks;
using Ozzy.Server;

namespace SampleApplication.Tasks
{
    public class TestBackgoundTask : BaseBackgroundTask
    {
        public override Task Execute(object taskConfig)
        {
            var configuration = taskConfig;
            Console.WriteLine(taskConfig.ToString());
            Thread.Sleep(2000);
            return Task.CompletedTask;                
        }
    }
}
