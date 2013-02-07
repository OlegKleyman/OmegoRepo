using System.IO;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class RarFileProcessor
    {
        public IFileSystem FileSystem { get; set; }
        private readonly IUnrarHandle _handle;

        public RarFileProcessor(IUnrarHandle handle, IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
            _handle = handle;
        }

        public RarStatus ProcessFile(string destinationPath, RARHeaderDataEx headerData)
        {
            var result =
                (RarStatus)
                _handle.UnrarDll.RARProcessFileW(_handle.Handle, (int)ArchiveMemberOperation.Extract, destinationPath,
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