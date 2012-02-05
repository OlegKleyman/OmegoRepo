namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class Release
    {
        public Release(ReleaseType releaseType)
        {
            ReleaseType = releaseType;
        }

        public ReleaseType ReleaseType { get; private set; }

        public string Name { get; set; }
    }
}