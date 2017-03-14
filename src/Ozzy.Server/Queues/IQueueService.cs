using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Queues
{
    public interface IQueueService<T> where T: class
    {
        void Add(T item);
        QueueItem<T> FetchNext();
        void Acknowledge(QueueItem<T> item);
    }
}
