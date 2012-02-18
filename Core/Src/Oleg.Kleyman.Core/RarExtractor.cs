using System;
using System.Diagnostics;
using System.IO;
using Oleg.Kleyman.Core.Configuration;

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

        public RarExtractor(IRarExtractorSettings settings)
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
            if (!File.Exists(UnrarPath))
            {
                const string cannotFindUnrarFileMessage = "Unable to find unrar file at location {0}";
                throw new FileNotFoundException(string.Format(cannotFindUnrarFileMessage, UnrarPath));
            }

            var processInfo = new ProcessStartInfo(UnrarPath, string.Format("e -y \"{0}\"", file.FullName));
            processInfo.WorkingDirectory = destination;
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            
            var process = Process.Start(processInfo);

            if(!process.HasExited)
            {
                process.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            process.WaitForExit();
        }
    }
}