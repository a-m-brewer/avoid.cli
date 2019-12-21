using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Avoid.Cli.Downloader.YoutubeDl
{
    public class YoutubeDownloadFactory : IDownloadFactory
    {
        private readonly IEnumerable<Action<object, DataReceivedEventArgs>> _callbacks;
        private readonly IEnumerable<Action<object, DataReceivedEventArgs>> _errorCallbacks;
        private readonly PathWrapper _pathWrapper;

        public YoutubeDownloadFactory(
            IEnumerable<Action<object, DataReceivedEventArgs>> callbacks,
            IEnumerable<Action<object, DataReceivedEventArgs>> errorCallbacks,
            PathWrapper pathWrapper = null)
        {
            _pathWrapper = pathWrapper ?? new PathWrapper();
            _callbacks = callbacks;
            _errorCallbacks = errorCallbacks;
        }

        public YoutubeDownloadFactory()
        {
            _pathWrapper = new PathWrapper();
        }
        
        public IRunnableProcess CreateDownload(string link, string location, bool audio, string format = null)
        {
            return CreateDownload(new List<string> { link }, location, audio, format);
        }

        public IRunnableProcess CreateDownload(IEnumerable<string> links, string location, bool audio, string format = null)
        {
            var program = new CliProgramBuilder();
            return program.Build(b =>
            {
                b.AddProgram("youtube-dl");
                foreach (var link in links)
                {
                    b.AddArgument(link);
                }

                b.AddFlagArgument("-o", _pathWrapper.Combine(location, "%(title)s.%(ext)s"));
                if (audio)
                {
                    b.AddFlag("-x");
                    b.AddFlagArgument("--audio-format", format ?? "mp3");
                }
                else
                {
                    if (format != null)
                    {
                        b.AddFlagArgument("--merge-output-format", format);
                    }
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