using System.Diagnostics;
using System.IO;
using Oleg.Kleyman.Core.Configuration;

namespace Oleg.Kleyman.Core
{
    public class RarExtractor : Extractor
    {
        private static readonly object __syncRoot;
        private static RarExtractor __defaultRarExtractor;
        public IFileSystem FileSystem { get; private set; }
        public IRarExtractorSettings Settings { get; private set; }
        protected IProcessManager ProcessManager { get; private set; }
        private readonly ProcessStartInfo _processInfo;

        static RarExtractor()
        {
            __syncRoot = new object();
        }

        public RarExtractor(string unrarPath, IFileSystem fileSystem, IProcessManager processManager) : this(new RarExtractorSettings(unrarPath), fileSystem, processManager)
        {
        }

        public RarExtractor(IRarExtractorSettings settings, IFileSystem fileSystem, IProcessManager processManager)
        {
            Settings = settings;
            FileSystem = fileSystem;
            ProcessManager = processManager;
            _processInfo = new ProcessStartInfo(settings.UnrarPath)
                               {
                                   CreateNoWindow = true,
                                   UseShellExecute = false,
                                   WindowStyle = ProcessWindowStyle.Hidden
                               };
        }

        private class RarExtractorSettings : IRarExtractorSettings
        {
            public RarExtractorSettings(string unrarPath)
            {
                UnrarPath = unrarPath;
            }

            #region Implementation of IRarExtractorSettings

            public string UnrarPath
            {
                get; private set;
            }

            #endregion
        }

        public static RarExtractor Default
        {
            get
            {
                lock (__syncRoot)
                {
                    if (__defaultRarExtractor == null)
                    {
                        __defaultRarExtractor = new RarExtractor(RarExtractorConfigurationSection.Default, new FileSystem(), new ProcessManager());
                    }
                }

                return __defaultRarExtractor;
            }
        }

        public override FileSystemInfo Extract(string target, string destination)
        {
            var file = FileSystem.GetFileByPath(target);

            ThrowFileNotFoundExceptionOnIfFileIsMissing();

            CreateDirectoryIfDoesNotExist(destination);

            ExtractArchive(destination, file);
            return FileSystem.GetDirectory(destination);
        }

        private void ExtractArchive(string destination, IFile file)
        {
            var process = StartProcess(destination, file);

            if (!process.HasExited)
            {
                process.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            process.WaitForExit();
        }

        private IProcess StartProcess(string destination, IFile file)
        {
            _processInfo.Arguments = string.Format("e -y \"{0}\"", file.FullName);
            _processInfo.WorkingDirectory = destination;
            var process = ProcessManager.Start(_processInfo);
            return process;
        }

        private void ThrowFileNotFoundExceptionOnIfFileIsMissing()
        {
            if (!FileSystem.FileExists(Settings.UnrarPath))
            {
                const string cannotFindUnrarFileMessage = "Unable to find unrar file at location {0}";
                throw new FileNotFoundException(string.Format(cannotFindUnrarFileMessage, Settings.UnrarPath));
            }
        }

        private void CreateDirectoryIfDoesNotExist(string destination)
        {
            if (!FileSystem.DirectoryExists(destination))
            {
                FileSystem.CreateDirectory(destination);
            }
        }
    }
}