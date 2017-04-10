using System;
using Ozzy.Core;
using Ozzy.DomainModel;

namespace Ozzy.Server.EntityFramework.Saga
{
    public class SagaRecord : AggregateBase<Guid>
    {
        public SagaRecord(SagaState sagaState) : base(sagaState.SagaId)
        {
            Guard.ArgumentNotNull(sagaState, nameof(sagaState));
            StateType = sagaState.State.GetType().AssemblyQualifiedName;
            SagaState = DefaultSerializer.Serialize(sagaState.State);
            SagaVersion = sagaState.SagaVersion;
            foreach (var message in sagaState.Messages)
            {
                this.RaiseEvent(message);
            }
        }

        // For ORM
        protected SagaRecord()
        {
        }

        public int SagaVersion { get; set; }
        public string StateType { get; set; }
        public string SagaState { get; set; }

        public SagaState ToSagaState()
        {
            var type = Type.GetType(StateType);
            var state = DefaultSerializer.Deserialize(SagaState, type);
            var sagaState = new SagaState(Id, state);
            sagaState.SagaVersion = SagaVersion;
            return sagaState;
        }
    }
}
