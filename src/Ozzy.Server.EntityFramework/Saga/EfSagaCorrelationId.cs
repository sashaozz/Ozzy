using System;

namespace Ozzy.Server.EntityFramework
{
    public class EfSagaCorrelationId
    {
        public string SagaType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid SagaId { get; set; }
        public virtual EfSagaRecord Saga { get; set; }

    }
}