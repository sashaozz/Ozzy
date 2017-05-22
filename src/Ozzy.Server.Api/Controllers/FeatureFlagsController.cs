using Microsoft.AspNetCore.Mvc;

namespace Ozzy.Server.Api.Controllers
{
    [Route("api/[controller]")]
    public class FeatureFlagsController : GenericDataController<FeatureFlag, string>
    {
        public FeatureFlagsController(IFeatureFlagRepository repository) : base(repository)
        {
        }        
    }
}
