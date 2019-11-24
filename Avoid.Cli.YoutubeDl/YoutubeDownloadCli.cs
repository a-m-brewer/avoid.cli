using System.Collections.Generic;

namespace Avoid.Cli.YoutubeDl
{
    public class YoutubeDownloadCli
    {
        private readonly IYoutubeDownloadFactory _downloadFactory;

        public YoutubeDownloadCli(IYoutubeDownloadFactory downloadFactory)
        {
            _downloadFactory = downloadFactory;
        }

        public bool Download(string link, string location, bool audio)
        {
            var process = _downloadFactory.CreateDownload(link, location, audio);
            return process.Start();
        }

        public bool Download(IEnumerable<string> links, string location, bool audio)
        {
            var process = _downloadFactory.CreateDownload(links, location, audio);
            return process.Start();
        }
    }
}