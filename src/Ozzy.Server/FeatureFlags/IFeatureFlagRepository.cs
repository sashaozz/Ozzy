using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public interface IFeatureFlagRepository : IDataRepository<FeatureFlag, string>
    {
    }
}
