using System.Threading.Tasks;

namespace Ozzy.Server
{
    public interface IBackgroundProcess
    {
        bool IsRunning { get; }
        string Name { get; }
        Task Start();
        void Stop();
    }
}