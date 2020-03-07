using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Avoid.Cli
{
    public class ProcessWrapper : Process, IProcess
    {
        public List<Action<IProcess>> PreprocessActions { get; } = new List<Action<IProcess>>();
        public List<Action<IProcess>> PostprocessActions { get; } = new List<Action<IProcess>>();
        
        public new bool Start()
        {
            foreach (var preprocessAction in PreprocessActions)
            {
                preprocessAction.Invoke(this);
            }
            
            var start = base.Start();
            BeginOutputReadLine();
            WaitForExit();

            foreach (var postprocessAction in PostprocessActions)
            {
                postprocessAction.Invoke(this);
            }

            return start;
        }
    }
}