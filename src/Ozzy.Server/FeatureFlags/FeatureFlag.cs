using System;
using Ozzy.DomainModel;
using Newtonsoft.Json;

namespace Ozzy.Server
{
    public class FeatureFlag : EntityBase<string>
    {
        [JsonProperty]
        public FeatureFlagConfiguration Configuration { get; private set; }

        [JsonIgnore]
        public byte[] SerializedConfiguration
        {
            get { return ContractlessMessagePackSerializer.Instance.BinarySerialize(Configuration); }
            protected set
            {
                if (value == null) return;
                Configuration = ContractlessMessagePackSerializer.Instance.BinaryDeSerialize<FeatureFlagConfiguration>(value);
            }
        }

        public FeatureFlag(string id) : base(id)
        {
            Configuration = new FeatureFlagConfiguration(false);
        }

        //For ORM
        protected FeatureFlag()
        {
        }

        public string GetVariation()
        {
            //todo: implement variations
            return IsEnabled().ToString();
        }

        public virtual bool IsEnabled()
        {
            return Configuration.IsEnabled;
        }

        public virtual void UpdateConfiguration(FeatureFlagConfiguration configuration, int version)
        {
            //todo: implement versioning
            if (version <= Version) return;
            Configuration = configuration;
        }
    }

    public class FeatureFlag<TVariation> : FeatureFlag
    {
        public FeatureFlag() : base("temp")
        {
            Id = this.GetType().FullName;
        }

        public virtual new TVariation GetVariation()
        {
            var variation = base.GetVariation();
            //todo: implement conversion/deserializtion
            throw new NotImplementedException();
        }
    }
}
