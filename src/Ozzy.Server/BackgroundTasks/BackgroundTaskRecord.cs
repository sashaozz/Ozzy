using Ozzy.Core;
using Ozzy.DomainModel;
using Newtonsoft.Json;
using System;

namespace Ozzy.Server.BackgroundTasks
{
    public class BackgroundTaskRecord : GenericDataRecord<string>
    {
        public BackgroundTaskRecord(string code) : base(code)
        {
  
        }
        // For ORM
        [JsonConstructor]
        protected BackgroundTaskRecord() { }

        public DateTime CreatedAt { get; set; }
    }
}
