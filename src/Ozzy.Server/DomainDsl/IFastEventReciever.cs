using System;

namespace Ozzy.DomainModel
{    
    public interface IFastEventReciever<TLoop> : IFastEventReciever where TLoop : DomainEventsManager
    {        
    }
}
