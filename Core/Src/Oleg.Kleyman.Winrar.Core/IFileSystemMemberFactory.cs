using System;
using Oleg.Kleyman.Core;

namespace Oleg.Kleyman.Winrar.Core
{
    /// <summary>
    /// Represents a <see cref="IFileSystemMember"/> factory.
    /// </summary>
    public interface IFileSystemMemberFactory
    {
        /// <summary>
        /// Gets the <see cref="IFileSystemMember"/> object for the <see cref="ArchiveMember"/>.
        /// </summary>
        /// <param name="archiveMember">The archive member to interface with</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns>A <see cref="IFileSystemMember"/> object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when either the archiveMember or destinationPath arguments are null.</exception>
        IFileSystemMember GetFileMember(ArchiveMember archiveMember, string destinationPath);
    }
}