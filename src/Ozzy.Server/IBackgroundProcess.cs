using System;
using Ozzy.Core;

namespace Ozzy.Server
{
    public interface IBackgroundProcess : IBackgroundTask
    {
        Guid Id { get; }
        bool IsRunning { get; }
        string Name { get; }
    }
}