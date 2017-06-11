namespace Ozzy.Server.Saga
{
    public class SagaCorrelationProperty
    {
        public SagaCorrelationProperty(string propertyName, string propertyValue)
        {            
            Guard.ArgumentNotNullOrEmptyString(propertyName, nameof(propertyName));
            Guard.ArgumentNotNullOrEmptyString(propertyValue, nameof(propertyValue));
            PropertyValue = propertyValue;
            PropertyName = propertyName;
        }
        
        public string PropertyName { get; protected set; }
        public string PropertyValue { get; protected set; }                  
    }
}
