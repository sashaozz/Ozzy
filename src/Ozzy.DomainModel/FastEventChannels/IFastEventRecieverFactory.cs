﻿
namespace Ozzy.DomainModel
{
    public interface IFastEventRecieverFactory
    {
        IFastEventReciever CreateReciever(DomainEventsLoop loop);// where TLoop : DomainEventsManager;
    }
}