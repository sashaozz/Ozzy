﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ozzy.Server.Configuration;
using System;

namespace Ozzy.Server.EntityFramework
{
    public static class EfOzzyServiceCollectionExtensions
    {
        public static OzzyDomainBuilder<TDomain> AddEntityFrameworkOzzyDomain<TDomain>(this IServiceCollection services)
            where TDomain : AggregateDbContext
        {
            var builder = services
                .AddOzzyDomain<TDomain>()
                .UseEntityFramework();
            return builder;
        }

        public static OzzyDomainBuilder<TDomain> AddEntityFrameworkOzzyDomain<TDomain>(this IServiceCollection services,
            Action<EfOzzyDomainOptionsBuilder<TDomain>> optionsAction = null)
            where TDomain : AggregateDbContext
        {
            var builder = services
                .AddOzzyDomain<TDomain>()
                .UseEntityFramework(optionsAction);
            return builder;
        }

        public static OzzyDomainBuilder<TDomain> AddEntityFrameworkOzzyDomain<TDomain>(this IServiceCollection services,
            Action<IServiceProvider, EfOzzyDomainOptionsBuilder<TDomain>> optionsAction)
            where TDomain : AggregateDbContext
        {
            var builder = services
                .AddOzzyDomain<TDomain>()
                .UseEntityFramework(optionsAction);
            return builder;
        }
    }
}