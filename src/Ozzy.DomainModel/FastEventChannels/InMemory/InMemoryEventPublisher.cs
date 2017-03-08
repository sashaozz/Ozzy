﻿using Ozzy.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ozzy.DomainModel
{   
    public class InMemoryEventPublisher : IFastEventPublisher
    {
        private InMemoryDomainEventsPubSub _domainQueue;

        public InMemoryEventPublisher(InMemoryDomainEventsPubSub domainQueue)
        {
            Guard.ArgumentNotNull(domainQueue, nameof(domainQueue));
            _domainQueue = domainQueue;
        }

        public void Publish(DomainEventRecord message)
        {
            _domainQueue.OnNext(message);
        }
    }
}