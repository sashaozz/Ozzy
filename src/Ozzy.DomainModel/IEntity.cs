using System;

namespace Ozzy.DomainModel
{
    public interface IEntity<T>
    {
        T Id { get; }
    }

    public interface IEntity
    {
    }
}
