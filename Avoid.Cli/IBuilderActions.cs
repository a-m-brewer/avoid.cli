using System;
using System.Diagnostics;

namespace Avoid.Cli
{
    public interface IBuilderActions
    {
        IBuilderActions AddProgram(string program);
        IBuilderActions AddFlagArgument(string flag, string argument);
        IBuilderActions AddArgument(string argument);
        IBuilderActions AddFlag(string flag);
        IBuilderActions AddDataReceivedCallback(Action<object, DataReceivedEventArgs> callback);
        IBuilderActions AddErrorReceivedCallback(Action<object, DataReceivedEventArgs> callback);
    }
}