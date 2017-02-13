using Ozzy.Core;
using System;

namespace Ozzy.Server.FeatureFlags
{
    public class FeatureFlag
    {
        public string Id { get; protected set; }
        public int Version { get;protected set; }
        public FeatureFlagConfiguration Configuration { get; private set; }

        public FeatureFlag(string code)
        {
            Guard.ArgumentNotNullOrEmptyString(code, nameof(code));
            Id = code;
            Configuration = new FeatureFlagConfiguration(false);
            Version = 0;
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
