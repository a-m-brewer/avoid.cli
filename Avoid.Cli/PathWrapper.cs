using System.IO;

namespace Avoid.Cli
{
    public class PathWrapper : IPathWrapper
    {
        public string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }
    }
}