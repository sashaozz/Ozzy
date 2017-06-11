using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Ozzy.Core;
using Ozzy.DomainModel;

namespace Ozzy.Server.Saga
{
    public class SagaCorrelationsMapper
    {
        private Dictionary<Type, SagaEventCorrelationsMapper> _sagaMappers;

        public SagaCorrelationsMapper()
        {
            _sagaMappers = new Dictionary<Type, SagaEventCorrelationsMapper>();
        }

        public SagaEventCorrelationsMapper GetMapper<TSaga>()
        {
            var sagaType = typeof(TSaga);
            if (!_sagaMappers.TryGetValue(sagaType, out var sagaMapper))
            {
                var sagaStateType = sagaType.GetTypeInfo().BaseType.GetGenericArguments()[0];
                var sagaMapperType = typeof(SagaEventCorrelationsMapper<>).MakeGenericType(sagaStateType);
                sagaMapper = Activator.CreateInstance(sagaMapperType, sagaType) as SagaEventCorrelationsMapper;
                _sagaMappers[sagaType] = sagaMapper;
            }
            return sagaMapper;
        }
    }

    public class SagaEventCorrelationConfiguration
    {
        public Func<object, object> EventPropertyFunc { get; set; }
        public Func<object, object> SagaPropertyFunc { get; set; }
        public string SagaPropertyName { get; set; }
    }

    public abstract class SagaEventCorrelationsMapper
    {
        protected Dictionary<Type, SagaEventCorrelationConfiguration> EventMappings;
        public Type SagaType { get; set; }
        public SagaEventCorrelationsMapper(Type sagaType)
        {
            SagaType = sagaType;
            EventMappings = new Dictionary<Type, SagaEventCorrelationConfiguration>();
        }

        public SagaCorrelationProperty GetCorrelationIdFromEvent(IDomainEvent domainEvent)
        {
            var eventType = domainEvent.GetType();
            if (EventMappings.TryGetValue(eventType, out var configuration))
            {
                var idValue = configuration.EventPropertyFunc(domainEvent);
                if (idValue == null) throw new InvalidOperationException($"Correlated Id for saga {SagaType.Name} cannot be null");
                return new SagaCorrelationProperty(configuration.SagaPropertyName, idValue.ToString());
            }
            return null;
        }

        public List<SagaCorrelationProperty> GetCorrelationIdsFromSaga(SagaBase saga)
        {
            if (saga.GetType() != SagaType) throw new InvalidOperationException("Wrong Saga Type");
            List<SagaCorrelationProperty> result = new List<SagaCorrelationProperty>();
            foreach (var config in EventMappings.Values.DistinctBy(v => v.SagaPropertyName))
            {
                var value = config.SagaPropertyFunc(saga.SagaState.State);
                if (value == null || object.Equals(GetDefault(value.GetType()), value)) continue;
                result.Add(new SagaCorrelationProperty(config.SagaPropertyName, value.ToString()));
            }
            return result;
        }

        static object GetDefault(Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }

    public class SagaEventCorrelationsMapper<TSagaState> : SagaEventCorrelationsMapper
    {
        public SagaEventCorrelationsMapper(Type sagaType) : base(sagaType)
        {
        }

        public void ConfigureCorrelationId<TEvent>(Expression<Func<TEvent, object>> eventPropertyExpression, Expression<Func<TSagaState, object>> sagaPropertyExpression) where TEvent : IDomainEvent
        {
            var eventMember = Reflect<TEvent>.GetMemberInfo(eventPropertyExpression, true);
            var eventProperty = eventMember as PropertyInfo;
            if (eventProperty == null)
            {
                throw new InvalidOperationException($"Mapping expressions for event members must point to properties. Change member {eventMember.Name} on {typeof(TEvent).FullName} to a property.");
            }
            var compiledEventPropertyExpression = eventPropertyExpression.Compile();
            var eventPropertyFunc = new Func<object, object>(o => compiledEventPropertyExpression((TEvent)o));

            var sagaMember = Reflect<TSagaState>.GetMemberInfo(sagaPropertyExpression, true);
            var sagaProperty = sagaMember as PropertyInfo;
            if (sagaProperty == null)
            {
                throw new InvalidOperationException($"Mapping expressions for saga members must point to properties. Change member {sagaMember.Name} on {typeof(TSagaState).FullName} to a property.");
            }
            var compiledSagaPropertyExpression = sagaPropertyExpression.Compile();
            var sagaPropertyFunc = new Func<object, object>(o => compiledSagaPropertyExpression((TSagaState)o));

            EventMappings.Add(typeof(TEvent), new SagaEventCorrelationConfiguration
            {
                EventPropertyFunc = eventPropertyFunc,
                SagaPropertyFunc = sagaPropertyFunc,
                SagaPropertyName = sagaProperty.Name
            });
        }


    }
}
