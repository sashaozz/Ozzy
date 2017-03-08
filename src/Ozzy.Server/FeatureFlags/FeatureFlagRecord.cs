using Ozzy.Core;
using Ozzy.DomainModel;
using Newtonsoft.Json;

namespace Ozzy.Server.FeatureFlags
{
    public class FeatureFlagRecord : GenericDataRecord<string>
    {
        [JsonProperty]        
        public FeatureFlagConfiguration Configuration { get; protected set; }

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
        public FeatureFlagRecord(string code, FeatureFlagConfiguration state) : base(code)
        {
            Configuration = state;
        }
        // For ORM
        [JsonConstructor]
        protected FeatureFlagRecord() { }
    }
}
