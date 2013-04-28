using System;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Winrar.Core
{
    public class DestinationPathBuilder : IPathBuilder
    {
        public IPathBuilder PathBuilder { get; private set; }

        public DestinationPathBuilder(IPathBuilder pathBuilder)
        {
            if (pathBuilder == null)
            {
                const string pathBuilderParamName = "pathBuilder";
                throw new ArgumentNullException(pathBuilderParamName);
            }
            PathBuilder = pathBuilder;
        }

        public string Build(string target1, string target2)
        {
            string finalPath;
            if (!string.IsNullOrEmpty(target1))
            {
                var targetPath = PathBuilder.Combine(target1, target2);
                finalPath = PathBuilder.GetFullPath(targetPath);
            }
            else
            {
                finalPath = null;
            }

            return finalPath;
        }

        public string Combine(string target1, string target2)
        {
            return PathBuilder.Combine(target1, target2);
        }

        public string GetFullPath(string targetPath)
        {
            return PathBuilder.GetFullPath(targetPath);
        }
    }
}