using System.Collections.Generic;
using System.Linq;

namespace Avoid.Cli
{
    public class AggregateRunnableProcess : IRunnableProcess
    {
        private readonly IEnumerable<IRunnableProcess> _processes;

        public AggregateRunnableProcess(IEnumerable<IRunnableProcess> processes)
        {
            _processes = processes;
        }
        
        public bool Start()
        {
            return _processes.All(a => a.Start());
        }
    }
}