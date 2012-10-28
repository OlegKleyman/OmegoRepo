using System.Diagnostics;
using System.IO;
using Oleg.Kleyman.Core.Configuration;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///   Represents a rar archive extractor.
    /// </summary>
    public class RarExtractor : Extractor
    {
        private static readonly object __syncRoot;
        private static RarExtractor __defaultRarExtractor;

        private readonly ProcessStartInfo _processInfo;

        static RarExtractor()
        {
            __syncRoot = new object();
        }

        /// <summary>
        ///   Initializes the <see cref="RarExtractor" /> object.
        /// </summary>
        /// <param name="unrarPath"> The path to the unrar executable. </param>
        /// <param name="fileSystem"> The <see cref="IFileSystem" /> object to interface with. </param>
        /// <param name="processManager"> The <see cref="IProcessManager" /> object to interface with. </param>
        public RarExtractor(string unrarPath, IFileSystem fileSystem, IProcessManager processManager)
            : this(new RarExtractorSettings(unrarPath), fileSystem, processManager)
        {
        }

        /// <summary>
        ///   Initializes the <see cref="RarExtractor" /> object.
        /// </summary>
        /// <param name="settings"> The <see cref="IRarExtractorSettings" /> object to use for this instance. </param>
        /// <param name="fileSystem"> The <see cref="IFileSystem" /> object to interface with. </param>
        /// <param name="processManager"> The <see cref="IProcessManager" /> object to interface with. </param>
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

        /// <summary>
        ///   Gets the <see cref="IFileSystem" /> object that this instance is using.
        /// </summary>
        public IFileSystem FileSystem { get; private set; }

        /// <summary>
        ///   Gets the settings for this instance.
        /// </summary>
        public IRarExtractorSettings Settings { get; private set; }

        /// <summary>
        ///   Gets the <see cref="IProcessManager" /> object for this instance.
        /// </summary>
        protected IProcessManager ProcessManager { get; private set; }

        /// <summary>
        ///   Gets the default <see cref="RarExtractor" /> object.
        /// </summary>
        public static RarExtractor Default
        {
            get
            {
                lock (__syncRoot)
                {
                    if (__defaultRarExtractor == null)
                    {
                        __defaultRarExtractor = new RarExtractor(RarExtractorConfigurationSection.Default,
                                                                 new FileSystem(), new ProcessManager());
                    }
                }

                return __defaultRarExtractor;
            }
        }

        /// <summary>
        ///   Extracts an object.
        /// </summary>
        /// <param name="target"> The path to the object to extract. </param>
        /// <param name="destination"> The extraction destination. </param>
        /// <returns> The extracted <see cref="IFileSystemMember" /> object. </returns>
        /// <exception cref="FileNotFoundException">Thrown when the unrar executable is not found.</exception>
        public override IFileSystemMember Extract(string target, string destination)
        {
            var file = FileSystem.GetFileByPath(target);

            ThrowFileNotFoundExceptionOnIfFileIsMissing();

            CreateDirectoryIfDoesNotExist(destination);

            ExtractArchive(destination, file);
            return FileSystem.GetDirectory(destination);
        }

        private void ExtractArchive(string destination, IFileSystemMember file)
        {
            var process = StartProcess(destination, file);

            if (!process.HasExited)
            {
                process.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            process.WaitForExit();
        }

        private IProcess StartProcess(string destination, IFileSystemMember file)
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

        #region Nested type: RarExtractorSettings

        private class RarExtractorSettings : IRarExtractorSettings
        {
            public RarExtractorSettings(string unrarPath)
            {
                UnrarPath = unrarPath;
            }

            #region Implementation of IRarExtractorSettings

            public string UnrarPath { get; private set; }

            #endregion
        }

        #endregion
    }
}