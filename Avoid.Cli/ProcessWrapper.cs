using System;
using System.Diagnostics;
using System.IO;

namespace Avoid.Cli
{
    public class ProcessWrapper : Process, IProcess
    {
        public new bool Start()
        {
            var start = base.Start();
            BeginOutputReadLine();
            WaitForExit();
            return start;
        }
    }
}