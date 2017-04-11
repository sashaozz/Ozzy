using Ozzy.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server
{
    public interface IDomainEventsManager
    {
        void AddDomainEvent(IDomainEvent domainEvent);
    }
}
