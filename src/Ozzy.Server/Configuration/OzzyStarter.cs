using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Ozzy.Server.Configuration
{
    public class OzzyStarter : IOzzyStarter
    {
        private IOzzyNode _node;
        public IApplicationBuilder Builder { get; }
        

        public OzzyStarter(IApplicationBuilder builder, IOzzyNode node)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (node == null) throw new ArgumentNullException(nameof(node));
            _node = node;
            Builder = builder;
        }
        
        public void Start()
        {
            _node.Start();
        }
    }
}
