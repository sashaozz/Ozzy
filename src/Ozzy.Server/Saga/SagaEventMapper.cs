using Ozzy.DomainModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.Saga
{
    public class SagaEventMapper
    {
        public SagaEventMapper()
        {
            _internalMapping = new Dictionary<string, Delegate>();
        }
        private Dictionary<string, Delegate> _internalMapping;

        public void ConfigureEventIdMapping<TSaga, TEvent>(Func<TEvent, string> idSelector)
            where TEvent : IDomainEvent
            where TSaga : SagaBase
        {
            _internalMapping.Add(typeof(TSaga).AssemblyQualifiedName + typeof(TEvent).AssemblyQualifiedName, idSelector);
        }

        public string GetSagaIdFromEvent<TSaga>(IDomainEvent domainEvent)
        {
            var key = typeof(TSaga).AssemblyQualifiedName + domainEvent.GetType().AssemblyQualifiedName;
            if (!_internalMapping.Keys.Any(k => k == key))
                return null;

            var rez = _internalMapping[key];
           
            return rez.DynamicInvoke(domainEvent) as string; //Йа костыль :'(
        }
    }
}
