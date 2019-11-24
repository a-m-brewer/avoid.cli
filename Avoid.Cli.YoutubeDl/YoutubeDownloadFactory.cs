using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Avoid.Cli.YoutubeDl
{
    public class YoutubeDownloadFactory : IYoutubeDownloadFactory
    {
        private readonly IEnumerable<Action<object, DataReceivedEventArgs>> _callbacks;
        private readonly IEnumerable<Action<object, DataReceivedEventArgs>> _errorCallbacks;

        public YoutubeDownloadFactory(IEnumerable<Action<object, DataReceivedEventArgs>> callbacks,
            IEnumerable<Action<object, DataReceivedEventArgs>> errorCallbacks)
        {
            _callbacks = callbacks;
            _errorCallbacks = errorCallbacks;
        }

        public YoutubeDownloadFactory()
        {
            
        }
        
        public IProcess CreateDownload(string link, string location, bool audio, string format = "mp3")
        {
            return CreateDownload(new List<string> { link }, location, audio, format);
        }

        public IProcess CreateDownload(IEnumerable<string> links, string location, bool audio, string format = "mp3")
        {
            var program = new CliProgramBuilder();
            return program.Build(b =>
            {
                b.AddProgram("youtube-dl");
                foreach (var link in links)
                {
                    b.AddArgument(link);
                }

                b.AddFlagArgument("-o", $"{location}\\%(title)s.%(ext)s");
                if (audio)
                {
                    b.AddFlag("-x");
                    b.AddFlagArgument("--audio-format", format);
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
        }
    }
}