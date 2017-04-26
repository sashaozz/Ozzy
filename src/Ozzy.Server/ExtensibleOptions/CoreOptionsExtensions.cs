using System;

namespace Ozzy.Server
{
    public static class CoreOptionsExtensions
    {        
        public static IServiceProvider GetServiceProvider(this IExtensibleOptions options)
        {
            var extension = options.FindExtension<CoreOptionsExtension>();
            return extension.ServiceProvider;
        }                
    }
}
