using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.Server.BackgroundTasks
{
    public abstract class BaseBackgroundTask
    {
        public string Id { get; set; }
        public abstract Task Execute();
        public string Content { get; set; }
    }

}
