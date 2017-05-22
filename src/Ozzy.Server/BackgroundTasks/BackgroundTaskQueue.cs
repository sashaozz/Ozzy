using System;
using System.Text;
using Newtonsoft.Json;

namespace Ozzy.Server
{
    public class BackgroundJobQueue : JobQueue<BackgroundTaskConfig>
    {
        public BackgroundJobQueue(ISerializer serializer, IQueueRepository queueRepository) : this(serializer, queueRepository, null)
        {
        }
        public BackgroundJobQueue(ISerializer serializer, IQueueRepository queueRepository, string queueName) : base(serializer, queueRepository, queueName)
        {
        }

        public string PutJob<TTask, TConfig>(TConfig taskConfig)
        {
            var payload = Serializer.BinarySerialize(taskConfig);
            var config = new BackgroundTaskConfig(typeof(TTask), typeof(TConfig), payload);
            return Put(config);
        }

        public string PutJob<TTask>(string taskConfig)
        {
            return PutJob<TTask, string>(taskConfig);
        }

        public bool FetchJob(out object jobConfig, out QueueItem<BackgroundTaskConfig> queueItem)
        {
            queueItem = Fetch();
            if (queueItem == null)
            {
                jobConfig = null;
                return false;
            }
            jobConfig = Serializer.BinaryDeSerialize(queueItem.Item.SerializedConfig, queueItem.Item.GetTaskConfigType());
            return true;
        }
    }
}
