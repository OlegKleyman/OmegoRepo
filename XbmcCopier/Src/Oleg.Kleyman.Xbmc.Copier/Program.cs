using System.IO;
using Oleg.Kleyman.Xbmc.Copier.Core;

namespace Oleg.Kleyman.Xbmc.Copier
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var writer = new StreamWriter(@"C:\Copier\copier.log", true))
            {
                writer.WriteLine("============================");
                writer.WriteLine(string.Format("Length: {0}", args.Length));
                foreach (string arg in args)
                {
                    writer.WriteLine(arg);
                }
                writer.WriteLine("============================");
                writer.WriteLine();
            }

            string downloadPath = args[0];
            string torrentName = args[1];
            string fileName = args.Length == 3 ? args[2] : null;


            string releaseName = torrentName;
            var builder = new ReleaseBuilder(releaseName);
            Release release = builder.Build();
            var copier = new XbmcFileCopier(new DefaultSettings(), release, fileName, downloadPath);
            if (release.ReleaseType == ReleaseType.Tv || release.ReleaseType == ReleaseType.Movie)
            {
                copier.Copy();
            }
        }
    }
}