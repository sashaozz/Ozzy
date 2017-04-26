using Newtonsoft.Json;
using System;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public class QueueRecord : EntityBase<string>
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
}
