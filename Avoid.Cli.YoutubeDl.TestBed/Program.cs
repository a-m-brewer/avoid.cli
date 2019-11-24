using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Avoid.Cli.YoutubeDl.TestBed
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloadCli = new YoutubeDownloadCli(new YoutubeDownloadFactory(new List<Action<object, DataReceivedEventArgs>>
            {
                (o, eventArgs) => Console.WriteLine(eventArgs.Data)
            }, new List<Action<object, DataReceivedEventArgs>>
            {
                (o, eventArgs) => Console.WriteLine(eventArgs.Data)
            }));

            downloadCli.Download("https://www.youtube.com/watch?v=b8HO6hba9ZE", "C:\\Users\\Adam\\Desktop", false);
            downloadCli.Download("https://www.youtube.com/watch?v=b8HO6hba9ZE", "C:\\Users\\Adam\\Desktop", true);
        }
    }
}