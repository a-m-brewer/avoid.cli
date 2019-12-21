using System.Collections.Generic;

namespace Avoid.Cli.Downloader
{
    public interface IDownloadFactory
    {
        IRunnableProcess CreateDownload(string link, string location, bool audio, string format = null);
        IRunnableProcess CreateDownload(IEnumerable<string> links, string location, bool audio, string format = null);
    }
}