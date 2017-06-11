using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public static class OzzySagaOptionsExtensions
    {
        public static ISagaRepository GetSagaRepository(this IExtensibleOptions options)
        {
            Guard.ArgumentNotNull(options, nameof(options));

            var extension = options.FindExtension<CoreOptionsExtension>();
            return extension.ServiceProvider.GetService<ISagaRepository>();
        }

        public static ICheckpointManager GetCheckpointManager(this IExtensibleOptions options, string serviceName)
        {
            Guard.ArgumentNotNull(options, nameof(options));
            Guard.ArgumentNotNullOrEmptyString(serviceName, nameof(serviceName));

            var extension = options.FindExtension<CoreOptionsExtension>();
            return extension.ServiceProvider.GetService<ICheckpointManager>();
        }
    }
}
