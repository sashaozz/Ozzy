using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Queues
{
    public class QueueItem<T>
    {
        public string QueueId { get; set; }
        public T Item { get; set; }
    }
}
