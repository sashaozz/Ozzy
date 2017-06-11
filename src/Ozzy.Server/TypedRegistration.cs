using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ozzy.Server
{
    public interface ITypedRegistration<TService>
    {
        TService GetService();
    }

    public class TypedRegistration<TType, TService> : ITypedRegistration<TService>
    {
        private TService _service;

        public TypedRegistration(TService service)
        {
            _service = service;
        }
        public TService GetService() => _service;
    }

}
