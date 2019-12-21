using System;
using System.Diagnostics;

namespace Avoid.Cli
{
    public interface IProcess : IRunnableProcess, IDisposable
    {
        ProcessStartInfo StartInfo { get; set; }
        event DataReceivedEventHandler OutputDataReceived;
        event DataReceivedEventHandler ErrorDataReceived;
    }
}