using System.IO;

namespace Oleg.Kleyman.Core
{
    public abstract class Extractor
    {
        public abstract FileSystemInfo Extract(string target, string destination);
    }
}