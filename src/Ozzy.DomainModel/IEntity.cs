using System;

namespace Ozzy.DomainModel
{
    public interface IEntity<T> : IEntity
    {
        T Id { get; }
        int Version { get; }
    }

    public interface IEntity
    {        
    }
}
