using System;
using System.Collections.Generic;
using System.IO;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class UnrarWrapper : IUnrarWrapper
    {
        private IUnrarDll UnrarDll { get; set; }

        /// <summary>
        /// Initializes the <see cref="UnrarWrapper"/> object.
        /// </summary>
        /// <param name="unrarDll">The <see cref="IUnrarDll"/> object to use for operations.</param>
        public UnrarWrapper(IUnrarDll unrarDll)
        {
            UnrarDll = unrarDll;
        }

        /// <summary>
        /// Opens an archive for operations.
        /// </summary>
        /// <param name="archivePath">The path to the archive.</param>
        /// <param name="mode">The mode to open the archive under.</param>
        /// <returns>A handle to the archive.</returns>
        public IntPtr Open(string archivePath, OpenMode mode)
        {
            var openData = new RAROpenArchiveDataEx
                {
                    ArcName = archivePath,
                    OpenMode = (uint)mode
                };

            var handle = UnrarDll.RAROpenArchiveEx(ref openData);

            if (((RarStatus) openData.OpenResult) != RarStatus.Success)
            {
                const string unableToOpenArchiveMessage = "Unable to open archive.";
                throw new UnrarException(unableToOpenArchiveMessage, (RarStatus)openData.OpenResult);
            }

            return handle;
        }

        /// <summary>
        /// Closes the handle.
        /// </summary>
        /// <param name="handle">The handle to the archive.</param>
        /// <returns>The status of the close operation.</returns>
        public RarStatus Close(IntPtr handle)
        {
            var status = (RarStatus) UnrarDll.RARCloseArchive(handle);

            if (status != RarStatus.Success)
            {
                const string unableToCloseArchiveMessage =
                    "Unable to close archive. Possibly because it's already closed.";
                throw new UnrarException(unableToCloseArchiveMessage, status);
            }

            return status;
        }

        /// <summary>
        /// Gets the files and folders inside the archive.
        /// </summary>
        /// <param name="handle">The handle to the archive.</param>
        /// <returns>The archive members.</returns>
        public IEnumerable<ArchiveMember> GetFiles(IntPtr handle)
        {
            RARHeaderDataEx headerData;

            var members = new List<ArchiveMember>();
            
            RarStatus result;
            
            while ((result = (RarStatus)UnrarDll.RARReadHeaderEx(handle, out headerData)) != RarStatus.EndOfArchive)
            {
                if (result != RarStatus.Success)
                {
                    const string unableToReadHeaderData = "Unable to read header data.";
                    throw new UnrarException(unableToReadHeaderData, result);
                }
                UnrarDll.RARProcessFileW(handle, (int)ArchiveMemberOperation.Extract, null, null);
                members.Add((ArchiveMember)headerData);
            }

            return members;
        }

        /// <summary>
        /// Etract the files in the archive.
        /// </summary>
        /// <param name="factory">The <see cref="IFileSystemMemberFactory"/> to get the extracted files from.</param>
        /// <param name="handle">The handle to the archive.</param>
        /// <param name="destinationPath">The destination folder to extract to.</param>
        /// <returns>The extracted files.</returns>
        public IEnumerable<IFileSystemMember> ExtractAll(IFileSystemMemberFactory factory, IntPtr handle, string destinationPath)
        {
            RARHeaderDataEx headerData;

            var members = new List<IFileSystemMember>();
            RarStatus result;

            while ((result = (RarStatus)UnrarDll.RARReadHeaderEx(handle, out headerData)) != RarStatus.EndOfArchive)
            {
                if (result != RarStatus.Success)
                {
                    const string unableToReadHeaderData = "Unable to read header data.";
                    throw new UnrarException(unableToReadHeaderData, result);
                }

                var fullExtractionPath = Path.GetFullPath(Path.Combine(destinationPath, headerData.FileNameW));
                UnrarDll.RARProcessFileW(handle, (int)ArchiveMemberOperation.Extract, null, fullExtractionPath);
                
                var member = factory.GetFileMember((ArchiveMember) headerData, destinationPath);
                members.Add(member);
            }
            
            return members;
        }
    }
}