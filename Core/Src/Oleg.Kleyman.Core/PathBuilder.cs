using System.IO;

namespace Oleg.Kleyman.Core
{
    public class PathBuilder : IPathBuilder
    {
        public string Build(string target, string target2)
        {
            return Combine(target, target2);
        }

        public string Combine(string target1, string target2)
        {
            return Path.Combine(target1, target2);
        }

        public string GetFullPath(string targetPath)
        {
            return Path.GetFullPath(targetPath);
        }
    }
}