using System;
using Newtonsoft.Json;

namespace Ozzy.DomainModel
{
    /// <summary>
    /// Базовый класс для всех IEntity доменной модели
    /// </summary>
    public abstract class EntityBase<T> : IEquatable<EntityBase<T>>, IEntity<T>
    {
        protected EntityBase(int version = 0)
        {
            Id = default(T);
            Version = version;
        }

        protected EntityBase(T id, int version = 0)
        {
            Guard.ArgumentNotNull(id, nameof(id));
            Id = id;
            Version = version;
        }

        [JsonProperty]
        public T Id { get; protected set; }
        [JsonProperty]
        public int Version { get; protected set; }

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
