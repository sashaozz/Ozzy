using Ozzy.Server.Queues;

namespace Ozzy.Server
{
    public class BackgroundTaskQueue : JobQueue<BackgroundTaskConfig>
    {
        public BackgroundTaskQueue(ISerializer serializer, IQueueRepository queueRepository, QueuesFaultManager queuesFaultManager) : this(serializer, queueRepository, queuesFaultManager, null)
        {
        }
        public BackgroundTaskQueue(ISerializer serializer, IQueueRepository queueRepository, QueuesFaultManager queuesFaultManager, string queueName) : base(serializer, queueRepository, queuesFaultManager, queueName)
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
