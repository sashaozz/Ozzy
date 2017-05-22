using System;

namespace Ozzy.Server
{
    public class CoreOptionsExtension : IOptionsExtension
    {
        public IServiceProvider ServiceProvider { get; set; }
    }
}
