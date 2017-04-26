using Ozzy.Core;
using System;
using Ozzy.DomainModel;
using Newtonsoft.Json;

namespace Ozzy.Server
{
    public class FeatureFlag : EntityBase<string>
    {
        //public string Id { get; protected set; }
        //public int Version { get;protected set; }
        public FeatureFlagConfiguration Configuration { get; private set; }

        [JsonIgnore]
        public string SerializedConfiguration
        {
            get { return DefaultSerializer.Serialize(Configuration); }
            protected set
            {
                if (string.IsNullOrEmpty(value)) return;
                Configuration = DefaultSerializer.Deserialize<FeatureFlagConfiguration>(value);
            }
        }

        public FeatureFlag(string code)
        {
            Guard.ArgumentNotNullOrEmptyString(code, nameof(code));
            Id = code;
            Configuration = new FeatureFlagConfiguration(false);
            Version = 0;
        }
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
