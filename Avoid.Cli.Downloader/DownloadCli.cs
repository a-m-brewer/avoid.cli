using System.Collections.Generic;

namespace Avoid.Cli.Downloader
{
    public class DownloadCli : IDownloadCli
    {
        private readonly IDownloadFactory _downloadFactory;

        public DownloadCli(IDownloadFactory downloadFactory)
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