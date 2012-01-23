using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Oleg.Kleyman.Xbmc.Copier.Core;

namespace Oleg.Kleyman.Xbmc.Copier
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var writer = new StreamWriter(@"C:\Copier\copier.log", true))
            {
                writer.WriteLine("============================");
                writer.WriteLine(string.Format("Length: {0}", args.Length));
                foreach (var arg in args)
                {
                    writer.WriteLine(arg);
                }
                writer.WriteLine("============================");
                writer.WriteLine();
            }

            var downloadPath = args[0];
            var torrentName = args[1];
            var fileName = args.Length == 3 ? args[2] : null;


            var releaseName = torrentName;
            var builder = new ReleaseBuilder(releaseName);
            var release = builder.Build();
            var copier = new XbmcFileCopier(new DefaultSettings(), release, fileName, downloadPath);
            if (release.ReleaseType == ReleaseType.Tv || release.ReleaseType == ReleaseType.Movie)
            {
                copier.Copy();
            }
        }
    }
}
