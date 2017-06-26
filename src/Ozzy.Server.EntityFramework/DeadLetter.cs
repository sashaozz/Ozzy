using Newtonsoft.Json;
using System;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public class DeadLetter : EntityBase<string>
    {
        public DeadLetter() : base(Guid.NewGuid().ToString())
        {
        }

        public DeadLetter(QueueItem item): base()
        {
            Payload = item.Payload;
            CreatedAt = item.CreatedAt;
            MaxRetries = item.MaxRetries;
            QueueName = item.QueueName;
            QueueItemId = item.Id;
        }

        public byte[] Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MaxRetries { get; set; }
        public string QueueName { get; set; }
        public string QueueItemId { get; set; }
    }
}
