using Newtonsoft.Json;
using System;

namespace Ozzy.DomainModel
{
    public abstract class GenericDataRecord<T> : AggregateBase<T>
    {
        protected GenericDataRecord()
        {

        }
        protected GenericDataRecord(T id) : base(id)
        {
        }

        //public DateTime LastUpdate { get; protected set; }
        [JsonProperty]
        public int Version { get; protected set; }        
    }
}
