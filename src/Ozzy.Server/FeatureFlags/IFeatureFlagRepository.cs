using Ozzy.DomainModel;

namespace Ozzy.Server.FeatureFlags
{
    public interface IFeatureFlagRepository : IDataRepository<FeatureFlagRecord, string>
    {
    }
}
