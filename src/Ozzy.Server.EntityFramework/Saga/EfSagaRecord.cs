using Ozzy.Server.Saga;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ozzy.Server.EntityFramework
{
    public class EfSagaRecord : AggregateBase<Guid>
    {
        public EfSagaRecord(SagaState sagaState, List<SagaCorrelationId> sagaCorrelationIds) : base(sagaState.SagaId)
        {
            var sagaStateType = sagaState.State.GetType();
            Guard.ArgumentNotNull(sagaState, nameof(sagaState));
            StateType = sagaStateType.AssemblyQualifiedName;
            SagaState = ContractlessMessagePackSerializer.Instance.BinarySerialize(sagaState.State, sagaStateType);
            SagaVersion = sagaState.SagaVersion;
            foreach (var message in sagaState.Messages)
            {
                this.RaiseEvent(message);
            }
            sagaState.Messages.Clear();
            CorrelationIds = sagaCorrelationIds.Select(id => new EfSagaCorrelationId() {
                SagaType = id.SagaType.Name,
                Name = id.PropertyName,
                Value = id.Value,
                SagaId = Id,
                Saga = this
            }).ToList();
        }

        // For ORM
        protected EfSagaRecord()
        {
        }

        public int SagaVersion { get; set; }
        public string StateType { get; set; }
        public byte[] SagaState { get; set; }
        public ICollection<EfSagaCorrelationId> CorrelationIds { get; set; }

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
