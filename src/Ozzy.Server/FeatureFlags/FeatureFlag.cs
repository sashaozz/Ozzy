using Ozzy.Core;
using System;

namespace Ozzy.Server.FeatureFlags
{
    public class FeatureFlag
    {
        public string Code { get; protected set; }
        public FeatureFlagConfiguration Configuration { get; private set; }

        public FeatureFlag(string code)
        {
            Guard.ArgumentNotNullOrEmptyString(code, nameof(code));
            Code = code;
            Configuration = new FeatureFlagConfiguration(false);
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

        public virtual void UpdateConfiguration(FeatureFlagConfiguration configuration)
        {
            //todo: implement versioning
            Configuration = configuration;
        }
    }

    public class FeatureFlag<TVariation> : FeatureFlag
    {
        public FeatureFlag() : base("temp")
        {
            Code = this.GetType().FullName;
        }

        public virtual new TVariation GetVariation()
        {
            var variation = base.GetVariation();
            //todo: implement conversion/deserializtion
            throw new NotImplementedException();
        }
    }
}
