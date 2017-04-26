﻿using Ozzy.DomainModel;

namespace Ozzy.Server
{
    public static class OzzyDomainOptionsnExtensions
    {        
        public static IFastEventPublisher GetFastEventPublisher<TDomain>(this IExtensibleOptions<TDomain> options)
        {
            return options.GetServiceProvider().GetTypeSpecificService<TDomain, IFastEventPublisher>();
        }

        public static IPeristedEventsReader GetPersistedEventsReader<TDomain>(this IExtensibleOptions<TDomain> options)
        {
            return options.GetServiceProvider().GetTypeSpecificService<TDomain,IPeristedEventsReader>();
        }
    }
}
