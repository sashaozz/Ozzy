using Ozzy.Core;

namespace Ozzy.Server
{
    public interface IBackgroundProcess : IBackgroundTask
    {
        bool IsRunning { get; }
        string Name { get; }        
    }
}