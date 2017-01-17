using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Ozzy.Server.Configuration
{
    public interface IOzzyStarter
    {
        IApplicationBuilder Builder { get; }
        void Start();
    }
}
