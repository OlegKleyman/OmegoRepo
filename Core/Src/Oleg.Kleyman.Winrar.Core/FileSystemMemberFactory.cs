using System;
using System.IO;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    /// Represents a file system factory for the <see cref="IFileSystem"/> interface.
    /// </summary>
    public class FileSystemMemberFactory
    {
        private IFileSystem _fileSystem;
        /// <summary>
        /// Gets or sets the file system interface for file operations.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the value being set is null.</exception>
        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
            set
            {
                if (value == null)
                {
                    const string valueParamName = "value";
                    throw new ArgumentNullException(valueParamName);
                }
                _fileSystem = value;
            }
        }

        /// <summary>
        /// Instantiates a <see cref="FileSystemMemberFactory"/> object.
        /// </summary>
        /// <param name="fileSystem">The <see cref="IFileSystem"/> object to interface with the file system.</param>
        /// <exception cref="ArgumentNullException">Thrown when the fileSystem argument is null.</exception>
        public FileSystemMemberFactory(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                const string fileSystemParamName = "fileSystem";
                throw new ArgumentNullException(fileSystemParamName);
            }
            FileSystem = fileSystem;
            
        }

        /// <summary>
        /// Gets the <see cref="IFileSystemMember"/> object for this instance.
        /// </summary>
        /// <param name="archiveMember">The archive member to interface with</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns>A <see cref="IFileSystemMember"/> object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when either the archiveMember or destinationPath arguments are null.</exception>
        public virtual IFileSystemMember GetFileMember(ArchiveMember archiveMember, string destinationPath)
        {
            if (archiveMember == null)
            {
                const string archiveMemberParamName = "archiveMember";
                throw new ArgumentNullException(archiveMemberParamName);
            }
            if (destinationPath == null)
            {
                const string destinationPathParamName = "destinationPath";
                throw new ArgumentNullException(destinationPathParamName);
            }

            var fileMemberPath = Path.Combine(destinationPath, archiveMember.Name);
            var fileMember = archiveMember.HighFlags == HighMemberFlags.DirectoryRecord 
                                 ? FileSystem.GetDirectory(fileMemberPath) 
                                 : FileSystem.GetFileByPath(fileMemberPath);
            return fileMember;
        }
    }
}