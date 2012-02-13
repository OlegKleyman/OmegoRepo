namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class ReleaseOutput
    {
        public Release Release { get; set; }
        public string FileName { get; set; }
        public string DownloadPath { get; set; }

        public ReleaseOutput(Release release, string fileName, string downloadPath)
        {
            Release = release;
            FileName = fileName;
            DownloadPath = downloadPath;
        }
    }
}