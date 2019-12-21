using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avoid.Cli.Downloader;
using Avoid.Cli.Downloader.YoutubeDl;

namespace Avoid.Cli.YoutubeDl.TestBed
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloadCli = new DownloadCli(new YoutubeDownloadFactory(new List<Action<object, DataReceivedEventArgs>>
            {
                (o, eventArgs) => Console.WriteLine(eventArgs.Data)
            }, new List<Action<object, DataReceivedEventArgs>>
            {
                (o, eventArgs) => Console.WriteLine(eventArgs.Data)
            }));

            downloadCli.Download("https://www.youtube.com/watch?v=b8HO6hba9ZE", "~/Desktop", false);
            downloadCli.Download("https://www.youtube.com/watch?v=b8HO6hba9ZE", "~/Desktop", true);
        }
    }
}