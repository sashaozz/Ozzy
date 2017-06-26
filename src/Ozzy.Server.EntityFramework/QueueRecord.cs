using Newtonsoft.Json;
using System;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public class QueueRecord : EntityBase<string>
    {
        public QueueRecord() : base(Guid.NewGuid().ToString())
        {
            Status = QueueStatus.Queued;
            CreatedAt = DateTime.UtcNow;
        }
        public byte[] Payload { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RetryCount { get; set; }
        public int MaxRetries { get; set; }
        public DateTime? TimeoutAt { get; set; }
        public QueueStatus Status { get; set; }
        public string QueueName { get; set; }
    }
}
