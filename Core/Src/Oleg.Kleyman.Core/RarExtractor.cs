using System;
using System.Diagnostics;
using System.IO;

namespace Oleg.Kleyman.Core
{
    public class RarExtractor : Extractor
    {
        private static object __syncRoot;
        private static RarExtractor __defaultRarExtractor;
        public IRarExtractorSettings Settings { get; private set; }

        static RarExtractor()
        {
            __syncRoot = new object();
        }

        public static RarExtractor Default
        {
            get
            {
                lock(__syncRoot)
                {
                    if (__defaultRarExtractor == null)
                    {
                        __defaultRarExtractor = new RarExtractor(RarExtractorConfigurationSection.Default);
                    }
                }

                return __defaultRarExtractor;
            }
        }
        public RarExtractor(string unrarPath)
        {
            UnrarPath = unrarPath.Trim();
        }

        private RarExtractor(IRarExtractorSettings settings)
        {
            Settings = settings;
        }

        protected string UnrarPath { get; private set; }

        public override void Extract(string target, string destination)
        {
            var currentDirectoryPath = Directory.GetCurrentDirectory();
            var file = new FileInfo(target);
            if (!Directory.Exists(destination))
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
            if (process == null)
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