using System.IO;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    ///   Represents a file system member.
    /// </summary>
    /// <remarks>
    ///   Such as a file or a directory.
    /// </remarks>
    public class FileSystemMember : IFileSystemMember
    {
        /// <summary>
        ///   Initializes a <see cref="FileSystemMember" /> object.
        /// </summary>
        /// <param name="fileSystemInfo"> The <see cref="FileSystemInfo" /> object to interface with. </param>
        public FileSystemMember(FileSystemInfo fileSystemInfo)
        {
            FileSystemInfo = fileSystemInfo;
        }

        private FileSystemInfo FileSystemInfo { get; set; }

        #region IFileSystemMember Members

        /// <summary>
        ///   Gets the full name of the member.
        /// </summary>
        public string FullName
        {
            get { return FileSystemInfo.FullName; }
        }

        /// <summary>
        ///   Gets the attributes of the member.
        /// </summary>
        public FileAttributes Attributes
        {
            get { return FileSystemInfo.Attributes; }
        }

        /// <summary>
        ///   Gets whether the member exists.
        /// </summary>
        public bool Exists
        {
            get { return FileSystemInfo.Exists; }
        }

        #endregion
    }
}