using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Ozzy.DomainModel;

namespace Ozzy.Server.Saga
{
    public static class OzzySagaOptionsExtensions
    {
        public static ISagaRepository GetSagaRepository(this IExtensibleOptions options)
        {
            var extension = options.FindExtension<CoreOptionsExtension>();
            return extension.ServiceProvider.GetService<ISagaRepository>();
        }

        public static ICheckpointManager GetCheckpointManager(this IExtensibleOptions options, string serviceName)
        {
            var extension = options.FindExtension<CoreOptionsExtension>();
            return extension.ServiceProvider.GetService<ICheckpointManager>();
        }
    }
}
