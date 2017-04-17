using Ozzy.DomainModel;
using Ozzy.Server.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ozzy.Server.Faults
{
    public class RetryEventTask : BaseBackgroundTask<RetryEventTaskParams>
    {
        private IServiceProvider _serviceProvider;

        public RetryEventTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task Execute()
        {
            var processor = _serviceProvider.GetService(this.ContentTyped.ProcessorType) as DomainEventsProcessor;
            // processor = null :'(
            if(processor != null)
            {
                try
                {
                    processor.HandleEvent(this.ContentTyped.Record);
                }
                catch (Exception ex)
                {
                    //Events processor will add a retry
                }
            }
        }
    }

    public class RetryEventTaskParams
    {
        public Type ProcessorType { get; set; }
        public DomainEventRecord Record { get; set; }
        public int RetryMaxCount { get; set; }
        public bool sendToErrorQueue { get; set; }
    }
}
