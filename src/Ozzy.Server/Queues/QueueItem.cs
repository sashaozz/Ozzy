using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Queues
{
    public class QueueItem<T>
    {
        public string Id { get; set; }
        public T Item { get; set; }
    }
}
