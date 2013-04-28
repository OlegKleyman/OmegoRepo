using System;
using System.Collections.Generic;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Core.Extensions;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    public class UnrarWrapper : IUnrarWrapper
    {
        private static readonly IntPtr r__closedHandle;
        private IUnrarDll UnrarDll { get; set; }
        private IPathBuilder PathBuilder { get; set; }
        private IntPtr Handle { get; set; }

        static UnrarWrapper()
        {
            r__closedHandle = default(IntPtr);
        }

        /// <summary>
        /// Initializes the <see cref="UnrarWrapper"/> object.
        /// </summary>
        /// <param name="unrarDll">The <see cref="IUnrarDll"/> object to use for operations.</param>
        /// <param name="pathBuilder"></param>
        public UnrarWrapper(IUnrarDll unrarDll, IPathBuilder pathBuilder)
        {
            UnrarDll = unrarDll;
            PathBuilder = pathBuilder;
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

            if ((RarStatus)openData.OpenResult != RarStatus.Success)
            {
                const string unableToOpenArchiveMessage = "Unable to open archive.";
                throw new UnrarException(unableToOpenArchiveMessage, (RarStatus)openData.OpenResult);
            }
            Handle = handle;
            return handle;
        }

        /// <summary>
        /// Closes the handle.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns>The status of the close operation.</returns>
        public RarStatus Close(IntPtr handle)
        {
            if (Handle == r__closedHandle)
            {
                const string handleNotOpenMessage = "Handle is not open.";
                throw new InvalidOperationException(handleNotOpenMessage);
            }
            var status = (RarStatus)UnrarDll.RARCloseArchive(Handle);

            if (status != RarStatus.Success)
            {
                const string unableToCloseArchiveMessage =
                    "Unable to close archive. Possibly because it's already closed.";
                throw new UnrarException(unableToCloseArchiveMessage, status);
            }
            Handle = r__closedHandle;
            return status;
        }

        /// <summary>
        /// Gets the files and folders inside the archive.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns>The archive members.</returns>
        public IEnumerable<ArchiveMember> GetFiles(IntPtr handle)
        {
            var members = new List<ArchiveMember>();
            ArchiveMember member;

            while ((member = GetNextMember(handle)) != null)
            {
                members.Add(member);
            }

            return members;
        }

        /// <summary>
        /// Extract the files in the archive.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="factory">The <see cref="IFileSystemMemberFactory"/> to get the extracted files from.</param>
        /// <param name="destinationPath">The destination folder to extract to.</param>
        /// <returns>The extracted files.</returns>
        public IEnumerable<IFileSystemMember> ExtractAll(IntPtr handle, IFileSystemMemberFactory factory, string destinationPath)
        {
            var members = new List<IFileSystemMember>();

            ArchiveMember archiveMember;
            while ((archiveMember = GetNextMember(destinationPath)) != null)
            {
                var member = factory.GetFileMember(archiveMember, destinationPath);
                members.Add(member);
            }

            return members;
        }

        private ArchiveMember GetNextMember(string destinationPath)
        {
            RARHeaderDataEx headerData;

            var result = (RarStatus)UnrarDll.RARReadHeaderEx(Handle, out headerData);

            if (result == RarStatus.EndOfArchive)
            {
                return null;
            }

            result.ThrowOnInvalidStatus(RarOperation.ReadHeader);
            var fullExtractionPath = PathBuilder.Build(destinationPath, headerData.FileNameW);
            
            result = (RarStatus) UnrarDll.RARProcessFileW(Handle, (int)ArchiveMemberOperation.Extract, null, fullExtractionPath);

            result.ThrowOnInvalidStatus(RarOperation.Process);

            return (ArchiveMember)headerData;
        }

        public ArchiveMember GetNextMember(IntPtr handle)
        {
            return GetNextMember(null);
        }
    }
}