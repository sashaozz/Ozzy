using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.DomainModel.Queues
{
    public class QueueRecord : GenericDataRecord<string>
    {
        public QueueRecord(string code) : base(code)
        {

        }
        // For ORM
        [JsonConstructor]
        protected QueueRecord() { }
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        public QueueStatus Status { get; set; }

        public string QueueName { get; set; }
        public string ItemType { get; set; }
    }

    public enum QueueStatus
    {
        Awaiting = 0,
        Processing = 1
    }
}
