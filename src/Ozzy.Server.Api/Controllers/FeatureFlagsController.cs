using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ozzy.Server.FeatureFlags;

namespace Ozzy.Server.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class FeatureFlagsController : GenericDataController<FeatureFlagRecord, string>
    {
        public FeatureFlagsController(IFeatureFlagRepository repository) : base(repository)
        {
        }        
    }
}
