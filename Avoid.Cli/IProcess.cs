using System;
using System.Diagnostics;

namespace Avoid.Cli
{
    public interface IProcess : IDisposable
    {
        bool Start();
        ProcessStartInfo StartInfo { get; set; }
        event DataReceivedEventHandler OutputDataReceived;
        event DataReceivedEventHandler ErrorDataReceived;
    }
}