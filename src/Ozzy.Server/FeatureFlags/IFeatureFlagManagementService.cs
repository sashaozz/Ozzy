using System;
using System.Collections.Generic;
using System.Text;

namespace Ozzy.Server.FeatureFlags
{
    public interface IFeatureFlagManagementService
    {    
        FeatureFlag GetFeatureFlag(string code);        
        List<FeatureFlag> GetAllFlags();
        //List<FeatureFlag> CreateFlag(string code, FeatureFlagConfiguration state);
        void SetFlagState(string code, FeatureFlagConfiguration state);
    }
}
