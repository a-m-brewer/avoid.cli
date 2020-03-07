using System.Collections.Generic;

namespace Avoid.Cli
{
    public static class ProcessExtensions
    {
        public static IRunnableProcess ToRunnableProcess(this IEnumerable<IProcess> processes)
        {
            return new AggregateRunnableProcess(processes);
        }

        public static IRunnableProcess ToRunnableProcess(this IProcess process)
        {
            return ToRunnableProcess(new[] {process});
        }
    }
}