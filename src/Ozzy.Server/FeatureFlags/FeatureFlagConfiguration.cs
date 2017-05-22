namespace Ozzy.Server
{
    public class FeatureFlagConfiguration
    {
        public FeatureFlagConfiguration(bool isEnabled = false)
        {
            IsEnabled = isEnabled;
        }       
        public bool IsEnabled { get; private set; }
    }
}
