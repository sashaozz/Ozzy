using Ozzy.DomainModel;
using System;

namespace Ozzy.Server.Saga
{
    public class SagaCorrelationId : ValueObject<SagaCorrelationId>
    {
        public SagaCorrelationId(Type sagaType, string propertyName, string value)
        {
            SagaType = sagaType;
            Value = value;
            PropertyName = propertyName;
        }        

        public SagaCorrelationId() { }
        public string Value { get; set; }
        public string PropertyName { get; set; }
        public Type SagaType { get; set; }        
    }
}
