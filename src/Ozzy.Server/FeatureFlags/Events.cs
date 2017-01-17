using Ozzy.DomainModel;

namespace Ozzy.Server.FeatureFlags
{
    public class FeatureFlagConfigurationCreated : IDomainEvent
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public FeatureFlagConfiguration Configuration { get; set; }
    }

    public class FeatureFlagConfigurationUpdated : IDomainEvent
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public FeatureFlagConfiguration Configuration { get; set; }
    }
}
