using System;
using Ozzy.Core;

namespace Ozzy.Server
{
    public interface IBackgroundProcess : IBackgroundTask
    {
        string ProcessId { get; }
        string ProcessName { get; }
        string ProcessState { get; }
        bool IsRunning { get; }
        
    }
}