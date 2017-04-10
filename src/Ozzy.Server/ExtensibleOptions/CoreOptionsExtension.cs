using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ozzy.Server
{
    public class CoreOptionsExtension : IOptionsExtension
    {
        public IServiceCollection ServiceCollection { get; set; } = new ServiceCollection();
        public IServiceProvider ServiceProvider { get; set; }
        public IServiceProvider TopLevelServiceProvider { get; set; }
    }
}
