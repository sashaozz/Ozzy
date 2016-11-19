using System;
using System.Reflection;

namespace Ozzy.DomainModel
{
    public class EntityRemovedEvent : IDomainEvent
    {
        public IEntity Entity { get; set; }
        public string EntityType { get; set; }

        public Type GetEntityType()
        {
            return Type.GetType(EntityType);
        }

        public bool ForAgregateType<T>()
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(GetEntityType());
        }

        /// <summary>
        /// Constructor for serialization
        /// </summary>
        public EntityRemovedEvent() { }

        public EntityRemovedEvent(IEntity entity)
        {
            Entity = entity;
            var entityType = entity.GetType();
            if (entityType.GetTypeInfo().BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                entityType = entityType.GetTypeInfo().BaseType;

            EntityType = entityType.Name;
        }
    }
}
