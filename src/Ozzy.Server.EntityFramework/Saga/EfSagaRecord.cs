using System;
using System.Linq;
using System.Collections.Generic;
using Ozzy.Server.Saga;

namespace Ozzy.Server.EntityFramework
{
    public class EfSagaRecord : AggregateBase<Guid>
    {
        public int SagaVersion { get; protected set; }
        public string StateType { get; protected set; }
        public byte[] SagaState { get; protected set; }
        public ICollection<EfSagaCorrelationId> CorrelationIds { get; protected set; }

        public EfSagaRecord(SagaBase saga, List<SagaCorrelationProperty> sagaCorrelationIds) : base(saga.SagaId)
        {
            Guard.ArgumentNotNull(saga, nameof(saga));
            Guard.ArgumentNotNull(sagaCorrelationIds, nameof(sagaCorrelationIds));
            var sagaState = saga.SagaState;
            var sagaStateType = sagaState.State.GetType();
            StateType = sagaStateType.AssemblyQualifiedName;
            SagaState = ContractlessMessagePackSerializer.Instance.BinarySerialize(sagaState.State, sagaStateType);
            SagaVersion = sagaState.SagaVersion;
            foreach (var message in sagaState.Messages)
            {
                this.RaiseEvent(message);
            }
            sagaState.Messages.Clear();
            CorrelationIds = sagaCorrelationIds.Select(id => new EfSagaCorrelationId(Id, saga.GetType().Name, id.PropertyName, id.PropertyValue)).ToList();
        }

        // For ORM
        protected EfSagaRecord()
        {
        }

        public SagaState ToSagaState()
        {
            var type = Type.GetType(StateType);
            var state = ContractlessMessagePackSerializer.Instance.BinaryDeSerialize(SagaState, type);
            var sagaState = new SagaState(Id, state);
            sagaState.SagaVersion = SagaVersion;
            return sagaState;
        }
    }
}
