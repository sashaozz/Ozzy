using System.Collections.Generic;

namespace Ozzy.Server.FeatureFlags
{
    public interface IFeatureFlagService
    {        
        TFeature GetFeatureFlag<TFeature>() where TFeature : FeatureFlag;
        bool IsEnabled<TFeature>() where TFeature : FeatureFlag;
        TVariation GetVariation<TFeature, TVariation>() where TFeature : FeatureFlag<TVariation>;

        FeatureFlag GetFeatureFlag(string code);
        bool IsEnabled(string code, bool defaultValue = false);
        string GetVariation(string code, string defaultValue = null);
    }
}