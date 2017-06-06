using System;

namespace Ozzy.Server.EntityFramework
{
    public class EfSagaRecord : AggregateBase<Guid>
    {
        public EfSagaRecord(SagaState sagaState) : base(sagaState.SagaId)
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
        }

        // For ORM
        protected EfSagaRecord()
        {
        }

        public int SagaVersion { get; set; }
        public string StateType { get; set; }
        public byte[] SagaState { get; set; }

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
