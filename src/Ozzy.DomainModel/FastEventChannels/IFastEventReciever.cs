using System;

namespace Ozzy.DomainModel
{    
    public interface IFastEventReciever : IDisposable
    {
        void Recieve(DomainEventRecord message);
        void StartRecieving();
        void StopRecieving();
    }
}
