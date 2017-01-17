using System;
using Ozzy.Core;
using Newtonsoft.Json;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Базовый класс для всех IEntity доменной модели
    /// </summary>
    public abstract class EntityBase<T> : IEquatable<EntityBase<T>>, IEntity<T>
    {
        protected EntityBase()
        {
            Id = default(T);
        }

        protected EntityBase(T id)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            Id = id;
        }

        [JsonProperty]
        public T Id { get; protected set; }        

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as EntityBase<T>);
        }

        public override int GetHashCode()
        {
            return (this.GetType().GetHashCode() * 907) + this.Id.GetHashCode();
        }

        public bool Equals(EntityBase<T> other)
        {
            if (object.ReferenceEquals(this, other)) return true;
            if (object.ReferenceEquals(null, other)) return false;
            return Id.Equals(other.Id);
        }

        public override string ToString()
        {
            return this.GetType().Name + " [Id=" + Id + "]";
        }
    }
}
