using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;
namespace Ozzy.Server
{
    public static class OzzyDomainOptionsnExtensions
    {
        public static IFastEventPublisher GetFastEventPublisher(this IExtensibleOptions options)
        {
            var extension = options.FindExtension<CoreOptionsExtension>();
            return extension.ServiceProvider.GetService<IFastEventPublisher>();
        }

        public static IFastEventPublisher GetFastEventPublisher<TDomain>(this IExtensibleOptions<TDomain> options)
        {
            var extension = options.FindExtension<CoreOptionsExtension>();
            return extension.ServiceProvider.GetService<IFastEventPublisher>();
        }

        public static IPeristedEventsReader GetPersistedEventsReader(this IExtensibleOptions options)
        {
            return options.GetService<IPeristedEventsReader>();
        }
    }
}
