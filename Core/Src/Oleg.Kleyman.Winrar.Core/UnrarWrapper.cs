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
            return handle;
        }

        /// <summary>
        /// Closes the handle.
        /// </summary>
        /// <param name="handle">The handle to the archive.</param>
        /// <returns>The status of the close operation.</returns>
        public RarStatus Close(IntPtr handle)
        {
            var status = UnrarDll.RARCloseArchive(handle);
            return (RarStatus)status;
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
            
            while ((RarStatus)UnrarDll.RARReadHeaderEx(handle, out headerData) != RarStatus.EndOfArchive)
            {
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
            
            while ((RarStatus)UnrarDll.RARReadHeaderEx(handle, out headerData) != RarStatus.EndOfArchive)
            {
                var fullExtractionPath = Path.GetFullPath(Path.Combine(destinationPath, headerData.FileNameW));
                UnrarDll.RARProcessFileW(handle, (int)ArchiveMemberOperation.Extract, null, fullExtractionPath);
                
                var member = factory.GetFileMember((ArchiveMember) headerData, destinationPath);
                members.Add(member);
            }
            
            return members;
        }
    }
}