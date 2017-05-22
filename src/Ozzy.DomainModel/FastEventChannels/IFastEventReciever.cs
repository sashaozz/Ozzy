using System;

namespace Ozzy.DomainModel
{    
    public interface IFastEventReciever : IDisposable
    {
        void Recieve(IDomainEventRecord message);
        void StartRecieving();
        void StopRecieving();
    }
}
