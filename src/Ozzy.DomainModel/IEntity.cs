using System;

namespace Ozzy.DomainModel
{
    public interface IEntity<T> : IEntity
    {
        T Id { get; }
    }

    public interface IEntity
    {        
    }
}
