using Ozzy.Server.BackgroundTasks;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ozzy.Server.Monitoring
{
    public class StartProcessTask : BaseBackgroundTask
    {
        private IServiceProvider _serviceProvider;

        public StartProcessTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public override Task Execute()
        {
            var ozzyNode = _serviceProvider.GetService<OzzyNode>();
            var processId = this.Content;
            var process = ozzyNode.BackgroundProcesses.FirstOrDefault(b => b.Name == processId);
            if (process != null)
                process.Start();

            return Task.CompletedTask;
        }
    }
}
