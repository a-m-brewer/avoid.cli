using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Avoid.Cli.Downloader.GetIplayer
{
    public class GetIplayerDownloadFactory : IDownloadFactory
    {
        private readonly IEnumerable<Action<object, DataReceivedEventArgs>> _callbacks;
        private readonly IEnumerable<Action<object, DataReceivedEventArgs>> _errorCallbacks;
        private PathWrapper _pathWrapper;

        public GetIplayerDownloadFactory(           
            IEnumerable<Action<object, DataReceivedEventArgs>> callbacks,
            IEnumerable<Action<object, DataReceivedEventArgs>> errorCallbacks,
            PathWrapper pathWrapper = null)
        {
            _pathWrapper = pathWrapper ?? new PathWrapper();
            _callbacks = callbacks;
            _errorCallbacks = errorCallbacks;
        }

        public GetIplayerDownloadFactory()
        {
            _pathWrapper = new PathWrapper();
        }
        
        public IRunnableProcess CreateDownload(string link, string location, bool audio, string format = null)
        {
            return CreateDownload(new List<string> {link}, location, audio, format);
        }

        public IRunnableProcess CreateDownload(IEnumerable<string> links, string location, bool audio, string format = null)
        {
            var processes = links.Select(s =>
            {
                var program = new CliProgramBuilder();

                return program.Build(b =>
                {
                    b.AddProgram("get-iplayer-python");
                    b.AddArgument(s);
                    b.AddFlagArgument("-l", location);

                    if (format != null)
                    {
                        b.AddFlagArgument("--output-format", format);
                    }

                    if (audio)
                    {
                        b.AddFlag("-a");
                    }
                    
                    if (_errorCallbacks != null)
                    {
                        foreach (var errorCallback in _errorCallbacks)
                        {
                            b.AddErrorReceivedCallback(errorCallback);
                        }
                    }

                    if (_callbacks == null) return;
                    foreach (var callback in _callbacks)
                    {
                        b.AddDataReceivedCallback(callback);
                    }
                });
            });
            
            return new AggregateRunnableProcess(processes);
        }
    }
}