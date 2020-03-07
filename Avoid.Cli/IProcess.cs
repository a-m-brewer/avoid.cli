using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Avoid.Cli
{
    public interface IProcess : IRunnableProcess, IDisposable
    {
        ProcessStartInfo StartInfo { get; set; }
        event DataReceivedEventHandler OutputDataReceived;
        event DataReceivedEventHandler ErrorDataReceived;
        
        List<Action<IProcess>> PreprocessActions { get; } 
        List<Action<IProcess>> PostprocessActions { get; } 
    }
}