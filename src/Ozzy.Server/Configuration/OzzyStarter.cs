using System;

namespace Ozzy.Server.Configuration
{
    public interface IOzzyStarter
    {
        void Start();
    }

    public class OzzyStarter : IOzzyStarter
    {
        private IOzzyNode _node;       

        public OzzyStarter(IOzzyNode node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            _node = node;
        }
        
        public void Start()
        {
            _node.Start();
        }
    }
}
