using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class ReleaseOutput : Output
    {
        public ReleaseOutput(string fileName, string targetDirectory, Release release) : base(fileName, targetDirectory)
        {
            Release = release;
            FileName = fileName;
            TargetDirectory = targetDirectory;
        }

        public Release Release { get; set; }
    }
}