using System;

namespace Avoid.Cli.TestBed
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new CliProgramBuilder();
            var process = program.Build(b =>
            {
                b.AddProgram("youtube-dl.exe");
                b.AddArgument("https://www.youtube.com/watch?v=b8HO6hba9ZE");
                b.AddFlagArgument("-o", "C:\\Users\\Adam\\Desktop\\%(title)s.%(ext)s");
                b.AddDataReceivedCallback(((o, eventArgs) => Console.WriteLine(eventArgs.Data)));
                b.AddErrorReceivedCallback((o, eventArgs) => Console.WriteLine(eventArgs.Data));
            });

            process.Start();
        }
    }
}