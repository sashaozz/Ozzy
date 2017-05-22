using System.Threading.Tasks;
using System;

namespace Ozzy.Server
{
    public abstract class BaseBackgroundTask
    {        
        public abstract Task Execute(object taskConfig);
    }

    public abstract class BaseBackgroundTask<T> : BaseBackgroundTask where T : class
    {
        public override Task Execute(object taskConfig)
        {
            T config = taskConfig as T;
            if (config == null) throw new InvalidOperationException("task type mismatch");
            return Execute(config);
        }
        public abstract Task Execute(T taskConfig);
    }
}
