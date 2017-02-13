using System;
using Ozzy.DomainModel;
using Ozzy.Core;

namespace Ozzy.Server.DomainDsl
{
    public interface ITypedRegistration<TType, TService>
    {
        TService GetService();
    }

    public class TypedRegistration<TType, TService>
    {
        private TService _service;

        public TypedRegistration(TService service)
        {
            _service = service;
        }
        public TService GetService() => _service;
    }
    
}
