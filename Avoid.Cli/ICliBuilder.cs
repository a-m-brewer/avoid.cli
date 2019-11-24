using System;
using System.Diagnostics;

namespace Avoid.Cli
{
    public interface ICliBuilder
    {
        IProcess Build(Action<IBuilderActions> configuration);
    }
}