using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    ///   Represents a store of Unrar operations.
    /// </summary>
    public class Unrar : IUnrar
    {
        /// <summary>
        ///   Initializes the <see cref="Unrar" /> object.
        /// </summary>
        /// <param name="handle"> The <see cref="IUnrarHandle" /> to use for operations. </param>
        /// <param name="fileSystem"> The <see cref="IFileSystem" /> to use for file system operations. </param>
        public Unrar(IUnrarHandle handle, IFileSystem fileSystem)
        {
            Handle = handle;
            FileSystem = fileSystem;
        }

        /// <summary>
        ///   The <see cref="IFileSystem" /> to use for file system operations.
        /// </summary>
        public IFileSystem FileSystem { get; set; }

        #region IUnrar Members

        /// <summary>
        ///   The <see cref="IUnrarHandle" /> to use for operations.
        /// </summary>
        public IUnrarHandle Handle { get; set; }

        /// <summary>
        ///   Invoked when a compressed member is extracted.
        /// </summary>
        public event EventHandler<UnrarEventArgs> MemberExtracted;

        /// <summary>
        ///   Executes the archive reader.
        /// </summary>
        /// <returns> An <see cref="IArchiveReader" /> . </returns>
        /// <exception cref="InvalidOperationException">Thrown when the handle is not open or the handle mode is
        ///   <see cref="OpenMode.Extract" />
        ///   .</exception>
        public IArchiveReader ExecuteReader()
        {
            ThrowExceptionOnHandleNull();
            var reader = new ArchiveReader(Handle);
            reader.ValidateHandle();
            return reader;
        }

        /// <summary>
        ///   Extracts the archive.
        /// </summary>
        /// <param name="destinationPath"> The destination folder to extract to. If it does not exist then it will be created. </param>
        /// <returns> A <see cref="FileSystemInfo" /> object containing directory information of the destination. </returns>
        /// <exception cref="InvalidOperationException">Thrown when the Handle or FileSystem properties are null.</exception>
        public IFileSystemMember[] Extract(string destinationPath)
        {
            ThrowExceptionOnHandleNull();
            ThrowExceptionOnFileSystemNull();

            return ExtractArchive(destinationPath);
        }

        #endregion

        protected void OnMemberExtracted(UnrarEventArgs e)
        {
            var handler = MemberExtracted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ThrowExceptionOnHandleNull()
        {
            if (Handle == null)
            {
                const string handleCannotBeNullMessage = "Handle cannot be null.";
                throw new InvalidOperationException(handleCannotBeNullMessage);
            }
        }

        private IFileSystemMember[] ExtractArchive(string destinationPath)
        {
            RarStatus result;
            var contents = new Collection<IFileSystemMember>();
            do
            {
                IFileSystemMember content;
                result = ExtractMember(destinationPath, out content);

                if (result != RarStatus.EndOfArchive)
                {
                    contents.Add(content);
                }
            } while (result != RarStatus.EndOfArchive);

            return contents.ToArray();
        }

        private RarStatus ExtractMember(string destinationPath, out IFileSystemMember content)
        {
            RARHeaderDataEx headerData;
            var result = (RarStatus) Handle.UnrarDll.RARReadHeaderEx(Handle.Handle, out headerData);
            content = default(IFileSystemMember);
            if (result == RarStatus.Success)
            {
                content = ProcessFile(destinationPath, headerData);
            }
            else if (result != RarStatus.EndOfArchive)
            {
                const string unableToReadHeaderData = "Unable to read header data.";
                throw new UnrarException(unableToReadHeaderData, result);
            }
            return result;
        }

        private IFileSystemMember ProcessFile(string destinationPath, RARHeaderDataEx headerData)
        {
            ProcessFile(destinationPath);

            var member = (ArchiveMember) headerData;
            var extractedPath = Path.Combine(destinationPath, member.Name);
            var content = GetExtractedPath(extractedPath, member.HighFlags);
            OnMemberExtracted(new UnrarEventArgs(member));
            return content;
        }

        private IFileSystemMember GetExtractedPath(string extractedPath, HighMemberFlags memberFlags)
        {
            IFileSystemMember content;
            if (memberFlags == HighMemberFlags.DirectoryRecord)
            {
                content = FileSystem.GetDirectory(extractedPath);
            }
            else
            {
                content = FileSystem.GetFileByPath(extractedPath);
            }

            return content;
        }

        private void ProcessFile(string destinationPath)
        {
            var result =
                (RarStatus)
                Handle.UnrarDll.RARProcessFileW(Handle.Handle, (int) ArchiveMemberOperation.Extract, destinationPath,
                                                null);
            if (result != RarStatus.Success)
            {
                const string unableToExtractFileMessage = "Unable to extract file.";
                throw new UnrarException(unableToExtractFileMessage, result);
            }
        }

        private void ThrowExceptionOnFileSystemNull()
        {
            if (FileSystem == null)
            {
                const string filesystemCannotBeNullMessage = "FileSystem cannot be null.";
                throw new InvalidOperationException(filesystemCannotBeNullMessage);
            }
        }
    }
}