using Ozzy.DomainModel;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ozzy.Server
{
    public class Test : IDomainEventRecord
    {
        private object _data;
        private Type _type;
        public Test()
        {
        }

        public Test(object data, Type type, long sequence)
        {
            _data = data;
            _type = type;
            Sequence = sequence;
        }
        public long Sequence { get; set; }

        public DateTime TimeStamp { get; set; }

        //public Dictionary<string, object> MetaData { get; set; } = new Dictionary<string, object>();

        public object GetDomainEvent()
        {
            return _data;
        }

        public T GetDomainEvent<T>()
        {
            return (T)_data;
        }

        public Type GetDomainEventType()
        {
            return _type;
        }
    }

    public class RetryEventTaskParams
    {
        public String ProcessorType { get; set; }
        public Test Record { get; set; }
        public int RetryMaxCount { get; set; }
        public bool sendToErrorQueue { get; set; }
    }

    public class RetryEventTask : BaseBackgroundTask<RetryEventTaskParams>
    {
        private IServiceProvider _serviceProvider;

        public RetryEventTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override Task Execute(RetryEventTaskParams taskConfig)
        {
            var type = Type.GetType(taskConfig.ProcessorType);
            var handler = _serviceProvider.GetService(type) as IDomainEventsHandler;
            // processor = null :'(
            if (handler != null)
            {
                try
                {
                    handler.HandleEvent(taskConfig.Record);
                }
                catch (Exception ex)
                {
                    var faultHandler = _serviceProvider.GetService<IDomainEventsFaultHandler>();
                    faultHandler.Handle(type, taskConfig.Record);
                    //Events processor will add a retry
                }
            }
            return Task.CompletedTask;
        }
    }
}
