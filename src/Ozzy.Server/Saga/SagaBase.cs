using Ozzy.Server.Saga;
using System;
using System.Collections.Generic;

namespace Ozzy.Server
{
    public abstract class SagaBase
    {
        public Guid SagaId { get; protected set; }
        public virtual SagaState SagaState { get; protected set; }
        public List<SagaKey> SagaKeys { get; set; } = new List<SagaKey>();
        public abstract void LoadSagaData(SagaState data);
        public void SendSagaCommand<T>(T command) where T : SagaCommand
        {
            SagaState.SendSagaCommand(command);
        }
        public virtual void ConfigureEventMappings(SagaEventMapper mapper)
        {

        }
    }

    public class SagaBase<TState> : SagaBase where TState : new()
    {
        public TState State { get; private set; }
        protected SagaBase()
        {
            var state = new SagaState(Guid.NewGuid(), new TState());
            LoadSagaData(state);
        }
        public override void LoadSagaData(SagaState data)
        {
            SagaState = data;
            SagaId = data.SagaId;
            State = (TState)data.State;
        }
    }
}
