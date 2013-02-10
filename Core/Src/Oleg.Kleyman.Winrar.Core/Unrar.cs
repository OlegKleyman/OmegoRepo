using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Oleg.Kleyman.Core;
using Oleg.Kleyman.Winrar.Interop;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    ///     Represents a store of Unrar operations.
    /// </summary>
    public class Unrar : IUnrar
    {
        public IFileSystemMemberFactory FileFactory { get; set; }

        /// <summary>
        ///     Initializes the <see cref="Unrar" /> object.
        /// </summary>
        /// <param name="handle">
        ///     The <see cref="IUnrarHandle" /> to use for operations.
        /// </param>
        /// <param name="extractor">The <see cref="IMemberExtractor"/> object to use for operations.</param>
        /// <param name="fileFactory">The <see cref="IFileSystemMemberFactory"/> object to use for operations.</param>
        public Unrar(IUnrarHandle handle, IMemberExtractor extractor, IFileSystemMemberFactory fileFactory)
        {
            Handle = handle;
            Extractor = extractor;
            FileFactory = fileFactory;
        }

        #region IUnrar Members

        /// <summary>
        ///     The <see cref="IUnrarHandle" /> to use for operations.
        /// </summary>
        public IUnrarHandle Handle { get; set; }

        public IMemberExtractor Extractor { get; set; }

        /// <summary>
        ///     Invoked when a compressed member is extracted.
        /// </summary>
        public event EventHandler<UnrarEventArgs> MemberExtracted;

        /// <summary>
        ///     Executes the archive reader.
        /// </summary>
        /// <returns>
        ///     An <see cref="IArchiveReader" /> .
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the handle is not open or the handle mode is
        ///     <see cref="OpenMode.Extract" />
        ///     .
        /// </exception>
        public IArchiveReader ExecuteReader()
        {
            ThrowExceptionOnHandleNull();
            var reader = new ArchiveReader(Handle);
            reader.ValidateHandle();
            return reader;
        }

        /// <summary>
        ///     Extracts the archive.
        /// </summary>
        /// <param name="destinationPath"> The destination folder to extract to. If it does not exist then it will be created. </param>
        /// <returns>
        ///     A <see cref="FileSystemInfo" /> object containing directory information of the destination.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown when the Handle or FileSystem properties are null.</exception>
        public IFileSystemMember[] Extract(string destinationPath)
        {
            ThrowExceptionOnHandleNull();
            ThrowExceptionOnFileSystemNull();

            return ExtractArchive(destinationPath);
        }

        #endregion

        protected virtual void OnMemberExtracted(UnrarEventArgs e)
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
            var contents = new Collection<IFileSystemMember>();

            while (Extractor.Extract(destinationPath) != RarStatus.EndOfArchive)
            {
                var fileMember = FileFactory.GetFileMember(Extractor.CurrentMember, destinationPath);
                contents.Add(fileMember);
                OnMemberExtracted(new UnrarEventArgs(Extractor.CurrentMember));
            }

            return contents.ToArray();
        }

        private void ThrowExceptionOnFileSystemNull()
        {
            if (FileFactory == null)
            {
                const string filesystemCannotBeNullMessage = "FileFactory cannot be null.";
                throw new InvalidOperationException(filesystemCannotBeNullMessage);
            }
        }
    }
}