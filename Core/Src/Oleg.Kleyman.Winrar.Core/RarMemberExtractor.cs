using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class RarMemberExtractor
    {
        public IUnrarHandle Handle { get; set; }
        public IFileSystem FileSystem { get; set; }

        public RarStatus Status { get; private set; }

        public RarMemberExtractor(IUnrarHandle handle, IFileSystem fileSystem)
        {
            Handle = handle;
            FileSystem = fileSystem;
        }

        public ArchiveMember Extract(string destinationPath)
        {
            RARHeaderDataEx headerData;
            var result = (RarStatus)Handle.UnrarDll.RARReadHeaderEx(Handle.Handle, out headerData);
            
            if (result == RarStatus.Success)
            {
                var fileProcessor = new RarFileProcessor(Handle, FileSystem);
                fileProcessor.ProcessFile(destinationPath, headerData);
            }
            else if (result != RarStatus.EndOfArchive)
            {
                const string unableToReadHeaderData = "Unable to read header data.";
                throw new UnrarException(unableToReadHeaderData, result);
            }
            Status = result;
            return (ArchiveMember)headerData;
        }
    }
}