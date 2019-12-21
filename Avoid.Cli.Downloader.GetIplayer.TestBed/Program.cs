using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Avoid.Cli.Downloader.GetIplayer.TestBed
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloadCli = new DownloadCli(new GetIplayerDownloadFactory(new List<Action<object, DataReceivedEventArgs>>
            {
                (o, eventArgs) => Console.WriteLine(eventArgs.Data)
            }, new List<Action<object, DataReceivedEventArgs>>
            {
                (o, eventArgs) => Console.WriteLine(eventArgs.Data)
            }));

            downloadCli.Download(new List<string>
            {
                "https://www.bbc.co.uk/programmes/m000cbh9",
                "https://www.bbc.co.uk/programmes/m000cbh7"
            }, "/home/adam/Desktop", false);

            downloadCli.Download("https://www.bbc.co.uk/programmes/m000c30y", "/home/adam/Desktop", true);
        }
    }
}