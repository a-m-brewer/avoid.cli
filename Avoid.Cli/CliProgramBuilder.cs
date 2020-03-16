using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Avoid.Cli
{
    public class CliProgramBuilder : IBuilderActions, ICliBuilder
    {
        private readonly IProcess _process;
        private string _program;
        private readonly List<string> _flags = new List<string>();
        private readonly Dictionary<string, string> _flagArguments = new Dictionary<string, string>();
        private readonly List<string> _arguments = new List<string>();
        private readonly List<Action<object, DataReceivedEventArgs>> _callbacks = new List<Action<object, DataReceivedEventArgs>>();
        private readonly List<Action<object, DataReceivedEventArgs>> _errorCallbacks = new List<Action<object, DataReceivedEventArgs>>();
        private readonly List<Action<IProcess>> _preprocessActions = new List<Action<IProcess>>();
        private readonly List<Action<IProcess>> _postprocessActions = new List<Action<IProcess>>();
        private readonly List<string> _allArgumentsInOrder = new List<string>();
        private bool _buildArgumentsInOrder = false;
        
        public CliProgramBuilder()
        {
            _process = new ProcessWrapper();
        }
        
        public IBuilderActions AddProgram(string program)
        {
            _program = program;
            return this;
        }
        
        public IBuilderActions AddFlagArgument(string flag, string argument)
        {
            _flagArguments.Add(flag, argument);
            _allArgumentsInOrder.AddRange(new List<string> {flag, argument});
            return this;
        }
        
        public IBuilderActions AddArgument(string argument, bool safeQuote = true)
        {
            var parsedArgument = safeQuote ? $"\"{argument}\"" : argument;
            _arguments.Add(parsedArgument);
            _allArgumentsInOrder.Add(parsedArgument);
            return this;
        }

        public IBuilderActions AddFlag(string flag)
        {
            _flags.Add(flag);
            _allArgumentsInOrder.Add(flag);
            return this;
        }

        public IBuilderActions AddDataReceivedCallback(Action<object, DataReceivedEventArgs> callback)
        {
            _callbacks.Add(callback);
            return this;
        }

        public IBuilderActions AddErrorReceivedCallback(Action<object, DataReceivedEventArgs> callback)
        {
            _errorCallbacks.Add(callback);
            return this;
        }

        public IBuilderActions AddPreprocessAction(Action<IProcess> action)
        {
            _preprocessActions.Add(action);
            return this;
        }

        public IBuilderActions AddPostprocessAction(Action<IProcess> action)
        {
            _postprocessActions.Add(action);
            return this;
        }

        public IBuilderActions BuildArgumentsInAddOrder()
        {
            _buildArgumentsInOrder = true;
            return this;
        }

        public IProcess Build()
        {
            DefaultSettings();
            _process.StartInfo.FileName = _program;
            var args = _buildArgumentsInOrder ? BuildArgumentsInOrder() : BuildArguments();
            _process.StartInfo.Arguments = args;
            _process.PreprocessActions.AddRange(_preprocessActions);
            _process.PostprocessActions.AddRange(_postprocessActions);
            return _process;
        }

        private string BuildArguments()
        {
            var arguments = _arguments.Any()
                ? _arguments
                    .Aggregate((current, addition) => $"{current} {addition}")
                : "";

            var flags = _flags.Any() ? _flags.Aggregate((current, addition) => $"{current} {addition}") : "";
            
            var flagArguments = _flagArguments.Any()
                ? _flagArguments
                    .Select(s => $"{s.Key} \"{s.Value}\"")
                    .Aggregate((current, addition) => $"{current} {addition}")
                : "";
            
            return $"{arguments} {flags} {flagArguments}";
        }

        private string BuildArgumentsInOrder()
        {
            return string.Join(" ", _allArgumentsInOrder);
        }

        private void RunCallbacks(object sender, DataReceivedEventArgs eventArgs)
        {
            ProcessCallbacks(_callbacks, sender, eventArgs);
        }
        
        private void RunErrorCallbacks(object sender, DataReceivedEventArgs eventArgs)
        {
            ProcessCallbacks(_errorCallbacks, sender, eventArgs);
        }

        private static void ProcessCallbacks(IEnumerable<Action<object, DataReceivedEventArgs>> callbacks, object sender,
            DataReceivedEventArgs eventArgs)
        {
            foreach (var callback in callbacks)
            {
                callback.Invoke(sender, eventArgs);
            }
        }

        private void DefaultSettings()
        {
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.CreateNoWindow = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.OutputDataReceived += RunCallbacks;
            _process.ErrorDataReceived += RunErrorCallbacks;
        }

        public IProcess Build(Action<IBuilderActions> configuration)
        {
            configuration.Invoke(this);
            return Build();
        }
    }
}