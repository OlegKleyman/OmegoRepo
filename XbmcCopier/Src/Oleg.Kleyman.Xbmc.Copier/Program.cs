﻿using System.IO;
using Oleg.Kleyman.Core;
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
            var builder = new ReleaseBuilder(XbmcCopierConfigurationSection.DefaultSettings, releaseName);
            var release = builder.Build();
            var output = new ReleaseOutput(fileName, downloadPath, release);
            var fileSystem = new FileSystem();
            var copier = new XbmcFileCopier(XbmcCopierConfigurationSection.DefaultSettings,
                                            new XbmcFileCopierDependencies(RarExtractor.Default, fileSystem));
            if (release.ReleaseType == ReleaseType.Tv || release.ReleaseType == ReleaseType.Movie)
            {
                copier.Copy(output);
            }
        }
    }
}