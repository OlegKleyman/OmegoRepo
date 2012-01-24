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

            if (!File.Exists(UnrarPath))
            {
                const string cannotFindUnrarFileMessage = "Unable to find unrar file at location {0}";
                throw new ApplicationException(string.Format(cannotFindUnrarFileMessage, UnrarPath));
            }

            var process = Process.Start(UnrarPath, string.Format("e -y \"{0}\"", file.FullName));
            if(process == null)
            {
                const string unableToOpenUnrarFileAtLocationMessage = "Unable to open unrar file at location {0}";
                throw new ApplicationException(string.Format(unableToOpenUnrarFileAtLocationMessage, UnrarPath));
            }
            process.PriorityClass = ProcessPriorityClass.BelowNormal;
            process.WaitForExit();
            Directory.SetCurrentDirectory(currentDirectoryPath);
        }
    }
}
