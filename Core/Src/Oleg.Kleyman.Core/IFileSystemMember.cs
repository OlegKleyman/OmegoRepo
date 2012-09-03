using System.IO;

namespace Oleg.Kleyman.Core
{
    /// <summary>
    /// Represents a file system member. 
    /// </summary>
    /// <remarks>Such as a file or a directory.</remarks>
    public interface IFileSystemMember
    {
        /// <summary>
        /// Gets the full name of the member.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the attributes of the member.
        /// </summary>
        FileAttributes Attributes { get; }

        /// <summary>
        /// Gets whether the member exists.
        /// </summary>
        bool Exists { get; }
    }
}