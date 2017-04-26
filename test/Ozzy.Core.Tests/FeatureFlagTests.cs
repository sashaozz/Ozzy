using Ozzy.Server;

namespace Ozzy.Core.Tests
{
    public class ConsoleLogFeature : FeatureFlag<bool> { }

    public class FeatureFlagTests
    {
        //[Fact]
        //public void Test1()
        //{
        //    var serviceCollection = new ServiceCollection();
        //    var serviceProvider = serviceCollection.BuildServiceProvider();
        //    var store = new InMemoryFeatureFlagStateProvider();
        //    var service1 = new FeatureFlagService(store, serviceProvider);
        //    var service2 = new FeatureFlagService(store, serviceProvider);

        //    var ff1 = service1.GetFeatureFlag<ConsoleLogFeature>();
        //    var ff2 = service2.GetFeatureFlag<ConsoleLogFeature>();
            
        //    if (ff1.IsEnabled())
        //    {
        //        //todo:something
        //    }
        //}
    }
}
