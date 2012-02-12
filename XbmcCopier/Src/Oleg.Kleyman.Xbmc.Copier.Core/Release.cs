namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class Release
    {
        public Release(ReleaseType releaseType, string name)
        {
            ReleaseType = releaseType;
            Name = name;
        }

        public ReleaseType ReleaseType { get; private set; }

        public string Name { get; private set; }
    }
}