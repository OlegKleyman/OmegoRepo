using System;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class RarFileProcessor
    {
        /// <summary>
        /// Gets the <see cref="IFileSystem"/>  provider for this object.
        /// </summary>
        public IFileSystem FileSystem { get; private set; }

        /// <summary>
        /// Gets the <see cref="IUnrarHandle"/> for this object.
        /// </summary>
        public IUnrarHandle Handle { get; private set; }

        /// <summary>
        /// Initializes an instance of <see cref="RarFileProcessor"/>.
        /// </summary>
        /// <param name="handle">The <see cref="IUnrarHandle"/> object to use for operations.</param>
        /// <param name="fileSystem">The <see cref="IFileSystem"/> object to use for operations.</param>
        /// <exception cref="ArgumentNullException">Thrown when either the handle or fileSystem argument is null.</exception>
        public RarFileProcessor(IUnrarHandle handle, IFileSystem fileSystem)
        {
            if (handle == null)
            {
                const string handleParamName = "handle";
                throw new ArgumentNullException(handleParamName);
            }

            if (fileSystem == null)
            {
                const string fileSystemParamName = "fileSystem";
                throw new ArgumentNullException(fileSystemParamName);
            }

            FileSystem = fileSystem;
            Handle = handle;
        }

        /// <summary>
        /// Processes a <see cref="RARHeaderDataEx"/> value.
        /// </summary>
        /// <param name="destinationPath">The target destination to extract to.</param>
        /// <returns>The extraction status.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the <see cref="IUnrarHandle"/> object is not open.</exception>
        /// <exception cref="UnrarException">Thrown when file could not be extracted.</exception>
        public RarStatus ProcessFile(string destinationPath)
        {
            if (!Handle.IsOpen)
            {
                const string handleMustBeOpenMessage = "Unrar handle must be open to complete this operation.";
                throw new InvalidOperationException(handleMustBeOpenMessage);
            }
            var result =
                (RarStatus)
                Handle.UnrarDll.RARProcessFileW(Handle.Handle, (int)ArchiveMemberOperation.Extract, destinationPath,
                                                null);
            if (result != RarStatus.Success)
            {
                const string unableToExtractFileMessage = "Unable to extract file.";
                throw new UnrarException(unableToExtractFileMessage, result);
            }

            return result;
        }
    }
}