using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Oleg.Kleyman.Xbmc.Copier.Core
{
    public class RarExtractor : Extractor
    {
        protected string UnrarPath { get; private set; }
        protected ISettingsProvider ConfigSettings { get; private set; }

        public RarExtractor(ISettingsProvider settings)
        {
            ConfigSettings = settings;
            UnrarPath = settings.UnrarPath;
        }

        public RarExtractor(string unrarPath)
        {
            UnrarPath = unrarPath;
        }

        public override void Extract(string target, string destination)
        {
            var currentDirectoryPath = Directory.GetCurrentDirectory();
            var file = new FileInfo(target);
            if(!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            Directory.SetCurrentDirectory(destination);
            
            var process = Process.Start(UnrarPath, string.Format("e -y \"{0}\"", file.FullName));
            process.PriorityClass = ProcessPriorityClass.BelowNormal;
            process.WaitForExit();
            Directory.SetCurrentDirectory(currentDirectoryPath);
        }
    }
}
