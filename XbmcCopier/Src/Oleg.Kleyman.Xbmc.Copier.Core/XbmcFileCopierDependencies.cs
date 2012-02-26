using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class XbmcFileCopierDependencies
    {
        public XbmcFileCopierDependencies(Extractor extractor, IFileSystem fileSystem)
        {
            Extractor = extractor;
            FileSystem = fileSystem;
        }

        public Extractor Extractor { get; set; }

        public IFileSystem FileSystem { get; set; }
    }
}