//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using Ozzy.DomainModel;
//using RedLock;
//using Ozzy.Core;

namespace AltLanDS.DomainModel.Redis
{
    //public class CompetingDomainQueueManager : WorkManager
    //{
    //    private readonly RedisLockFactory _rlockFactory;
    //    private readonly IPeristedEventsReader _eventsReader;
    //    private readonly ICheckpointManager _checkpointManager;
    //    private readonly IEnumerable<IDomainEventHandler> _handlers;
    //    private readonly int _batchSize;

    //    public CompetingDomainQueueManager(RedisLockFactory rlockFactory,
    //        IPeristedEventsReader eventsReader,
    //        ICheckpointManager checkpointManager,
    //        IEnumerable<IDomainEventHandler> handlers,
    //        int batchSize = 100) : base(1, 1000, -1)
    //    {
    //        _rlockFactory = rlockFactory;
    //        _eventsReader = eventsReader;
    //        _checkpointManager = checkpointManager;
    //        _handlers = handlers;
    //        _batchSize = batchSize;

    //    }

    //    protected override void Work(CancellationToken stopRequestedToken)
    //    {
    //        var resource = "the-thing-we-are-locking-on";
    //        var expiry = TimeSpan.FromSeconds(10);
    //        using (var redisLock = _rlockFactory.Create(resource, expiry))
    //        {
    //            // make sure we got the lock
    //            if (redisLock.IsAcquired)
    //            {
    //                var checkpoint = _checkpointManager.GetCheckpoint();
    //                var events = _eventsReader.GetEvents(checkpoint, _batchSize);
    //                foreach (var domainEventRecord in events)
    //                {
    //                    foreach (var domainEventHandler in _handlers)
    //                    {
    //                        domainEventHandler.HandleEvent(domainEventRecord);
    //                    }
    //                    checkpoint++;
    //                    _checkpointManager.SaveCheckpoint(checkpoint);
    //                }
    //            }
    //        }
    //    }
    //}
}
