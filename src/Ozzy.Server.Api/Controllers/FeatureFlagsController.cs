using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Ozzy.Server.FeatureFlags;

namespace Ozzy.Server.Api.Controllers
{
    [Route("api/[controller]")]
    public class FeatureFlagsController : GenericDataController<FeatureFlagRecord, string>
    {
        public FeatureFlagsController(IFeatureFlagRepository repository) : base(repository)
        {
        }
        //private IFeatureFlagService _ffService;

        //public FeatureFlagsController(IFeatureFlagService ffService)
        //{
        //    _ffService = ffService;
        //}

        //// Get all FFs
        //[HttpGet]
        //public IEnumerable<FeatureFlag> Get()
        //{
        //    var flags = _ffService.GetAllFlags();
        //    return flags;
        //}

        //// Get FF by id
        //[HttpGet("{id}")]
        //public FeatureFlag Get(string id)
        //{
        //    return _ffService.GetFeatureFlag(id);
        //}

        //// Create FF
        //[HttpPost]
        //public void Post([FromBody]FeatureFlag flag)
        //{
        //    _ffService.GetFeatureFlag(flag.Code);
        //}

        //// Update FF
        //[HttpPut("{id}")]
        //public void Put(string id, [FromBody]FeatureFlag flag)
        //{
        //    _ffService.SetFlagState(id, flag.Configuration);
        //}

        //// Archive FF
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

    }
}
