using System.Threading;
using System.Threading.Tasks;
using Ozzy.Server;

namespace SampleApplication.Tasks
{
    public class TestBackgoundTask : BaseBackgroundTask
    {
        public override async Task Execute()
        {
            var configuration = this.Content;
            Thread.Sleep(2000);
        }
    }
}
