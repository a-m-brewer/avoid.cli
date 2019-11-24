using System.Collections.Generic;

namespace Avoid.Cli.YoutubeDl
{
    public interface IYoutubeDownloadFactory
    {
        IProcess CreateDownload(string link, string location, bool audio, string format = "mp3");
        IProcess CreateDownload(IEnumerable<string> links, string location, bool audio, string format = "mp3");
    }
}