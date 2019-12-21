using System.Collections.Generic;

namespace Avoid.Cli.Downloader
{
    public interface IDownloadCli
    {
        bool Download(string link, string location, bool audio);
        bool Download(IEnumerable<string> links, string location, bool audio);
    }
}