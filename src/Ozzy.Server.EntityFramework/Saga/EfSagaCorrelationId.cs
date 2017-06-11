using System;

namespace Ozzy.Server.EntityFramework
{
    public class EfSagaCorrelationId
    {        
        public Guid SagaId { get; protected set; }
        public string SagaType { get; protected set; }
        public string PropertyName { get; protected set; }
        public string PropertyValue { get; protected set; }

        public virtual EfSagaRecord Saga { get; set; }

        public EfSagaCorrelationId(Guid sagaId, string sagaType, string propertyName, string propertyValue)
        {
            Guard.ArgumentNotNull(sagaId, nameof(sagaId));
            Guard.ArgumentNotNull(sagaType, nameof(sagaType));
            Guard.ArgumentNotNull(propertyName, nameof(propertyName));
            Guard.ArgumentNotNull(propertyValue, nameof(propertyValue));

            SagaId = sagaId;
            SagaType = sagaType;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        //For ORM
        protected EfSagaCorrelationId() { }        
    }
}