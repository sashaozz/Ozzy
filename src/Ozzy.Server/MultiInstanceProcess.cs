using System.Threading.Tasks;

namespace Ozzy.Server
{
    public class MultiInstanceProcess : BackgroundProcessBase
    {
        protected override Task StartInternal()
        {
            return base.StartInternal();
        }
    }
}
