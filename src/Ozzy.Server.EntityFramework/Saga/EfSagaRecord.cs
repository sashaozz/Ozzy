using Ozzy.Server.Saga;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Ozzy.Server.EntityFramework
{
    public class EfSagaRecord : AggregateBase<Guid>
    {
        public EfSagaRecord(SagaState sagaState, List<SagaKey> sagaKeys) : base(sagaState.SagaId)
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
            SagaKeys = sagaKeys.Select(s => new EfSagaKey() { Value = s.Value, Id = s.Id }).ToList();
        }

        // For ORM
        protected EfSagaRecord()
        {
        }

        public int SagaVersion { get; set; }
        public string StateType { get; set; }
        public byte[] SagaState { get; set; }

        public ICollection<EfSagaKey> SagaKeys { get; set; }

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
